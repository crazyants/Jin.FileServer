using System.Collections.Generic;

namespace FileServer.Application.Dtos.Glo
{
    /// <summary>
    /// 删除文件命令
    /// </summary>
    public class RemoveFileCmd
    {
        /// <summary>
        /// 文件路径集合[https://localhost:44317/upload/product/201806/1.jpg_800x.jpg]
        /// </summary>
        public List<string> FilePaths { get; set; }
    }
}