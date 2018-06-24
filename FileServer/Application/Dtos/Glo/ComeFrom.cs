using System.ComponentModel;

namespace FileServer.Application.Dtos.Glo
{
    /// <summary>
    /// 文件来源
    /// </summary>
    public enum ComeFrom
    {
        /// <summary>
        /// 总后台
        /// </summary>
        [Description("s1")]
        总后台 = 1,

        /// <summary>
        /// 来源于商户后台
        /// </summary>
        [Description("s2")]
        商户后台 = 2,

        /// <summary>
        /// 代理后台
        /// </summary>
        [Description("s3")]
        代理后台 = 3,

        /// <summary>
        /// 供应商后台
        /// </summary>
        [Description("s4")]
        供应商后台 = 4,

        /// <summary>
        /// 连锁总后台
        /// </summary>
        [Description("s5")]
        连锁总后台 = 5,
    }
}