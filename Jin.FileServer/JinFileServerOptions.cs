using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace Jin.FileServer
{
    public class JinFileServerOptions
    {
        /// <summary>
        /// 获取或设置允许动态调整大小的图片格式
        /// </summary>
        public List<string> ImageFormats = new List<string> { ".bmp", ".gif", ".jpg", ".jpeg", ".png" };

        /// <summary>
        /// 获取或设置允许动态生成的图片大小
        /// </summary>
        public List<Size> ImageSizes = new List<Size>
        {
            //不合适，80和800在一起有毛用啊
            //80, 100, 200, 480, 500, 600, 800
            new Size(80,80),
            new Size(100,100),
            new Size(200,200),
            new Size(480,480),
            new Size(500,500),
            new Size(600,600),
            new Size(800,800),
        };

        /// <summary>
        /// 未授权图片路径
        /// </summary>
        public string UnauthorizedImage { get; set; } = "/upload/unauthorized.jpg";

        /// <summary>
        /// 空表示空的来源，.表示允许当前主机名
        /// </summary>
        public List<string> AllowedDomains { get; set; } = new List<string> { "", "." };

        /// <summary>
        /// 为了不失真填充图片时，使用的背景色
        /// </summary>
        public Rgba32 BackgroundColor { get; set; } = Rgba32.White;


        /// <summary>
        /// 判断指定大小的图片是否满足要求
        /// </summary>
        public bool Contains(int? width, int? height)
        {
            return ImageSizes.Exists(n =>
                       (width == null || width == n.Width) &&
                       (height == null || height == n.Height));
        }

        /// <summary>
        /// 判断请求来源是否被允许
        /// </summary>
        public bool IsRefererAllowed(string referer, HttpContext context)
        {
            var self = AllowedDomains.FirstOrDefault(n => n == ".");
            if (self != null)
            {
                AllowedDomains.RemoveAll(n => n == ".");
                AllowedDomains.Insert(0, context.Request.Host.Value);
            }
            return AllowedDomains.Exists(n => n == referer);
        }

    }
}