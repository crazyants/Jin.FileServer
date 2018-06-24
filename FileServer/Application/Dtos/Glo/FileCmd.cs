namespace FileServer.Application.Dtos.Glo
{
    /// <summary>
    /// 文件操作命令基类
    /// </summary>
    public class FileCmd
    {
        /// <summary>
        /// 图片来源[很重要，每个来源对应一个存储文件夹]
        /// </summary>
        public ComeFrom ComeFrom { get; set; }
    }
}