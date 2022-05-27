using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KF.ElasticSearch.Config
{
    public class EsConfig
    {
        //服务地址
        public string Urls { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        public List<Uri> Uris => Urls.Split(',').Select(x => new Uri(x)).ToList();
    }
}
