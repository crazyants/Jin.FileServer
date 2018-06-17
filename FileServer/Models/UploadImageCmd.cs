using Microsoft.AspNetCore.Http;

namespace FileServer.Models
{
    /// <summary>
    /// 上传文件命令
    /// </summary>
    public class UploadImageCmd
    {
        /// <summary>
        /// 图片来源
        /// </summary>
        public ComeFrom ComeFrom { get; set; }

        /// <summary>
        /// 文件集合
        /// </summary>
        public IFormCollection Files { get; set; }
    }
}