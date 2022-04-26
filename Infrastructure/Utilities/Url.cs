using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Infrastructure.Utilities
{
    /// <summary>
    /// Url操作
    /// </summary>
    public static class Url
    {
        /// <summary>
        /// 合并Url
        /// </summary>
        /// <param name="urls">url片断，范例：Url.Combine( "http://a.com","b" ),返回 "http://a.com/b"</param>
        /// <returns></returns>
        public static string Combine(params string[] urls)
        {
            return Path.Combine(urls).Replace(@"\", "/");
        }
        /// <summary>
        /// 连接Url，范例：Url.Join( "http://a.com","b=1" ),返回 "http://a.com?b=1"
        /// </summary>
        /// <param name="url">url，范例：http://a.com</param>
        /// <param name="parm">参数，范例：b=1</param>
        /// <returns></returns>
        public static string join(string url, string parm)
        {
            return $"{GetUrl(url)}{parm}";
        }
        /// <summary>
        /// 获取Url
        /// </summary>
        private static string GetUrl(string url)
        {
            if (!url.Contains("?"))
                return $"{url}?";
            if (url.EndsWith("?"))
                return url;
            if (url.EndsWith("&"))
                return url;
            return $"{url}&";
        }
    }
}
