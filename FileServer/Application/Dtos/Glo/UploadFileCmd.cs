using Microsoft.AspNetCore.Http;

namespace FileServer.Application.Dtos.Glo
{
    /// <summary>
    /// 文件服务器上传文件命令
    /// </summary>
    public class UploadFileCmd : FileCmd
    {
        /// <summary>
        /// 文件集合
        /// </summary>
        public IFormCollection Files { get; set; }

        /// <summary>
        /// 文件路径集合，多个用,分割[传文件时，无法使用集合来接收文件名，故使用分割的字符串]
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 上传的文件类型[默认是图片文件]
        /// </summary>
        public FileType FileType { get; set; } = FileType.Image;
    }
}