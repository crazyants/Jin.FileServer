using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Overlays;
using SixLabors.ImageSharp.Processing.Transforms;
using SixLabors.Primitives;

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
            if (!_jinFileServerOptions.ImageFormats.Contains(entension))
            {
                await _next(context);
                return;
            }
            //请求图片，验证防盗链
            var referer = context.Request.Headers[HeaderNames.Referer].ToString();
            //从外部网页内部嵌入图片，进行验证
            if (!_jinFileServerOptions.IsRefererAllowed(referer, context.Request.Host.Host))
            {
                var unauthorizedImage = _env.WebRootFileProvider.GetFileInfo(_jinFileServerOptions.UnauthorizedImage);
                if (unauthorizedImage.Exists)
                    await context.Response.SendFileAsync(unauthorizedImage);
                else
                    await _next(context);
                return;
            }

            var destPath = context.Request.Path.Value;

            //如果目标图片存在，直接输出
            var destImage = _env.WebRootFileProvider.GetFileInfo(destPath);
            if (destImage.Exists)
            {
                await context.Response.SendFileAsync(destImage);
                return;
            }

            var destFileName = Path.GetFileName(destPath);

            //不希望调整图片，进入下一个中间件 //希望调整图片的参数重复，进入下一个中间件
            if (destFileName.IndexOf('_') == -1 ||
                destFileName.IndexOf('_') != destFileName.LastIndexOf('_'))
            {
                await _next(context);
                return;
            }
            

            var prefix = destFileName.Substring(0, destFileName.IndexOf('_'));
            var srcImage = _env.WebRootFileProvider.GetFileInfo(prefix);
            //原图不存在，进入下一个中间件
            if (!srcImage.Exists)
            {
                await _next(context);
                return;
            }

            var affix = destFileName.Substring(destFileName.IndexOf('_') + 1);
            //原图存在，生成目标图片，再输出
            var fileSize = affix.Substring(0, affix.IndexOf('.'));
            var fileExt = Path.GetExtension(affix);

            //调整图片大小
            //调整原则：【保证图片不变形！！！】
            using (var image = Image.Load(srcImage.CreateReadStream()))
            {
                var size = new Size();
                if (!fileSize.Contains('x'))
                {
                    size.Width = size.Height = Convert.ToInt32(fileSize);
                }
                else if (fileSize.StartsWith('x'))
                {
                    //固定高度缩放
                    size.Height = Convert.ToInt32(fileSize.Substring(1));
                    size.Width = (int)(image.Width * 1.0m / image.Height * size.Height);
                    //验证目标大小是否允许
                    if (!_jinFileServerOptions.Contains(null, size.Height))
                    {
                        await _next(context);
                        return;
                    }
                }
                else if (fileSize.EndsWith('x'))
                {
                    //固定宽度缩放
                    size.Width = Convert.ToInt32(fileSize.Substring(0, fileSize.Length - 1));
                    size.Height = (int)(size.Width / (image.Width * 1.0m / image.Height));
                    //验证目标大小是否允许
                    if (!_jinFileServerOptions.Contains(size.Width, null))
                    {
                        await _next(context);
                        return;
                    }
                }
                else
                {
                    size.Width = Convert.ToInt32(fileSize.Split('x')[0]);
                    size.Height = Convert.ToInt32(fileSize.Split('x')[1]);
                    //验证目标大小是否允许
                    if (!_jinFileServerOptions.Contains(size.Width, size.Height))
                    {
                        await _next(context);
                        return;
                    }
                }

                ResizeWithoutDistortion(image, size, fileExt == ".png");
                //保存调整后的图片
                image.Save(_env.WebRootPath + destPath);
            }
            //加载目标图片
            destImage = _env.WebRootFileProvider.GetFileInfo(destPath);
            await context.Response.SendFileAsync(destImage);
        }
        /// <summary>
        /// 无失真调整大小
        /// </summary>
        public void ResizeWithoutDistortion(Image<Rgba32> srcImage, Size destSize, bool isPng)
        {
            var srcRadio = srcImage.Width * 1.0m / srcImage.Height;
            var destRadio = destSize.Width * 1.0m / destSize.Height;

            //不失真大小
            var undistortedSize = new Size();
            if (destRadio > srcRadio)
            {
                //固定高度缩放
                undistortedSize.Height = destSize.Height;
                undistortedSize.Width = (int)Math.Round(srcRadio * undistortedSize.Height, 0, MidpointRounding.AwayFromZero);
            }
            else
            {
                //固定宽度缩放
                undistortedSize.Width = destSize.Width;
                undistortedSize.Height = (int)Math.Round(undistortedSize.Width / srcRadio, 0, MidpointRounding.AwayFromZero);
            }

            //调整到不失真大小
            srcImage.Mutate(x => x.Resize(undistortedSize));
            //填充到目的宽高
            srcImage.Mutate(x => x
                .Pad(destSize.Width, destSize.Height)
                .BackgroundColor(isPng ? _jinFileServerOptions.PngBackgroundColor : _jinFileServerOptions.BackgroundColor)
            );
        }
    }
}