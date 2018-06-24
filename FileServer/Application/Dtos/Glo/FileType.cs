namespace FileServer.Application.Dtos.Glo
{
    /// <summary>
    /// 上传的文件类型
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// 图片文件
        /// </summary>
        Image = 1,

        /// <summary>
        /// 文档文件（docx，可能需要验证内容合法性）
        /// </summary>
        Document = 2,

        /// <summary>
        /// 附件文件
        /// </summary>
        Attachment = 3,
    }
}