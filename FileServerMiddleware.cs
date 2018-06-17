using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;

namespace Jin.FileServer
{
    /// <summary>
    /// 文件服务器中间件
    /// </summary>
    public class FileServerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostingEnvironment _env;
        private readonly JinFileServerOptions _jinFileServerOptions = new JinFileServerOptions();

        public FileServerMiddleware(RequestDelegate next, IHostingEnvironment env, Action<JinFileServerOptions> action)
        {
            _next = next;
            _env = env;
            action(_jinFileServerOptions);
        }

        /// <summary>
        /// 每次请求都会调用
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            //全站请求的是图片文件
            var entension = Path.GetExtension(context.Request.Path.Value);
            if (_jinFileServerOptions.ImageFormats.Contains(entension))
            {
                var destPath = context.Request.Path.Value;
                //如果目标图片存在，直接输出
                var destImage = _env.WebRootFileProvider.GetFileInfo(destPath);
                if (destImage.Exists)
                {
                    await context.Response.SendFileAsync(destImage);
                    return;
                }

                //调整图片后输出
                if (destPath.IndexOf('_') > -1)
                {
                    var prefix = destPath.Substring(0, destPath.IndexOf('_'));
                    var srcImage = _env.WebRootFileProvider.GetFileInfo(prefix);
                    //原图存在
                    if (srcImage.Exists)
                    {
                        try
                        {
                            var affix = destPath.Substring(destPath.IndexOf('_') + 1);
                            //原图存在，生成目标图片，再输出
                            var fileSize = affix.Substring(0, affix.IndexOf('.'));
                            var fileExt = affix.Substring(affix.IndexOf('.'));

                            var width = Convert.ToInt32(fileSize.Split('x')[0]);
                            var height = Convert.ToInt32(fileSize.Split('x')[1]);
                            var size = new Size(width, height);

                            if (_jinFileServerOptions.ImageSizes == null || _jinFileServerOptions.ContainsSize(size))
                            {
                                //调整图片大小
                                using (var image = Image.Load(srcImage.CreateReadStream()))
                                {
                                    image.Mutate(x => x.Resize(size.Width, size.Height));
                                    //保存调整后的图片
                                    image.Save(_env.WebRootPath + destPath);
                                }
                                //加载目标图片
                                destImage = _env.WebRootFileProvider.GetFileInfo(destPath);
                                await context.Response.SendFileAsync(destImage);
                                return;
                            }
                        }
                        catch
                        {
                            throw new Exception("图片生成参数格式有误！");
                        }
                    }
                }
            }

            //执行下一个中间件
            await _next(context);
        }
    }
}