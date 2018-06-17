using System.Collections.Generic;
using System.Drawing;

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
            new Size(80, 80),
            new Size(100, 100),
            new Size(200, 200),
            new Size(480, 480),
            new Size(500, 500),
            new Size(600, 600),
            new Size(800, 800),
        };

        /// <summary>
        /// 判断图片大小中是否包含指定的大小
        /// </summary>
        public bool ContainsSize(Size size)
        {
            foreach (var imageSize in ImageSizes)
            {
                if (imageSize.Width == size.Width && imageSize.Height == size.Height)
                    return true;
            }
            return false;
        }
    }
}