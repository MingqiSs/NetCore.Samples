using Infrastructure.Const;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Utilities
{
    public class HashOpt
    {
        /// <summary>
        /// 获取Hash求余
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int GetHashQY(object obj, int y)
        {
            var hashCode = GetStringHashCode(obj.ToString());
            return Math.Abs(hashCode) % y;
        }
        /// <summary>
        /// 获取K线表名
        /// </summary>
        /// <param name="period"></param>
        /// <param name="obj">股票编码为int类型，指数string类型</param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static string GetTableName(string period, object obj, int y)
        {
            int opt = 0;
            switch (period)
            {
                case "1m":
                    opt = 1;
                    break;
                case "5m":
                    opt = 2;
                    break;
                case "15m":
                    opt = 3;
                    break;
                case "30m":
                    opt = 4;
                    break;
                case "1h":
                    opt = 5;
                    break;
                case "4h":
                    opt = 6;
                    break;
                case "1d":
                    opt = 7;
                    break;
                case "wk":
                    opt = 8;
                    break;
                case "mh":
                    opt = 9;
                    break;
            }
            if (opt == 0)
            {
                return "";
            }
            int n = GetHashQY(obj, y);
            return $"KLine_{n}_{opt}";
        }
        private static int GetStringHashCode(string text)
        {
            unchecked
            {
                int hash = 23;
                foreach (char c in text)
                {
                    hash = hash * 31 + c;
                }
                return hash;
            }
        }
    }
}
