using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FileServer.Infrastructure.Utils
{
    /// <summary>
    /// 枚举辅助类
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 把枚举转换成列表
        /// </summary>
        public static List<EnumObject> ToList<T>()
        {
            var list = new List<EnumObject>();
            var values = Enum.GetValues(typeof(T));
            foreach (var value in values)
            {
                var enumObject = new EnumObject { Value = Convert.ToInt32(value) };
                object[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                //没有自定义特性
                if (customAttributes.Length == 0)
                    enumObject.Text = value.ToString();
                //有自定义特性
                else if (customAttributes[0] is DescriptionAttribute descriptionAttribute)
                    enumObject.Text = descriptionAttribute.Description;
                list.Add(enumObject);
            }
            return list;
        }

        /// <summary>
        /// 获取指定枚举值的描述
        /// </summary>
        public static string Description<T>(T t)
        {
            var description = string.Empty;
            var values = Enum.GetValues(typeof(T));
            foreach (T value in values)
            {
                if (value.ToString() == t.ToString())
                {
                    object[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                    //没有自定义特性
                    if (customAttributes.Length == 0)
                        description = value.ToString();
                    //有自定义特性
                    else if (customAttributes[0] is DescriptionAttribute descriptionAttribute)
                        description = descriptionAttribute.Description;

                    break;
                }
            }
            return description;
        }
    }

    /// <summary>
    /// 枚举容器对象
    /// </summary>
    public class EnumObject
    {
        /// <summary>
        /// 枚举值
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// 枚举文本（描述->枚举名）
        /// </summary>
        public string Text { get; set; }
    }
}