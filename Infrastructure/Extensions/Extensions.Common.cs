using System;

namespace Infrastructure.Models
{
    /// <summary>
    /// 系统扩展 - 公共
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 安全获取值，当值为null时，不会抛出异常
        /// </summary>
        /// <param name="value">可空值</param>
        public static T SafeValue<T>(this T? value) where T : struct
        {
            return value ?? default(T);
        }
        /// <summary>
        /// 安全转换为字符串，去除两端空格，当值为null时返回""
        /// </summary>
        /// <param name="input">输入值</param>
        public static string SafeString(this object input)
        {
            return input == null ? string.Empty : input.ToString().Trim();
        }
        /// <summary>
        ///格式化decimal类型小数点后的无效0
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FormatDecimal(this decimal input)
        {
            // input.ToString("0.##");
            return string.Format("{0:0.##}", input);
        }
        /// <summary>
        ///安全格式化decimal类型小数点后的无效0
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FormatDecimal(this decimal? input)
        {
            // input.ToString("0.##");
            return input == null ? string.Empty : string.Format("{0:0.##}", input);
        }
        /// <summary>
        /// 获取枚举值
        /// </summary>
        /// <param name="instance">枚举实例</param>
        public static int Value(this System.Enum instance)
        {
            Type type = instance.GetType();
            string value = instance.SafeString();
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(instance));
            return (int)System.Enum.Parse(type, instance.ToString(), true);

        }
    }
}
