using System;
using System.Text;
using MassTransit;

namespace FileServer.Infrastructure.Utils
{
    /// <summary>
    /// 字符串生成器
    /// </summary>
    public class StringGenerator
    {
        /// <summary>
        /// 生成新的单号
        /// </summary>
        public static string OrderNo(string prefix)
        {
            var random = new Random().Next(10, 99);
            return prefix + DateTime.Now.ToString("yyMMddHHmmssfff") + random;
        }

        /// <summary>
        /// 生成指定长度的随机数字
        /// </summary>
        public static string Numeric(int length)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < length; i++)
                sb.Append(new Random().Next(0, 10).ToString());
            return sb.ToString();
        }

        /// <summary>
        /// 生成随机唯一的文件名
        /// </summary>
        public static string FileName(string extension)
        {
            return NewId.Next().ToString("N") + extension;
        }
    }
}