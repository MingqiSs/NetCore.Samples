using Infrastructur.AutofacManager;
using Infrastructure.AutofacManager;
using Infrastructure.Utilities;
using Nacos;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Config
{
    public interface INacosAppsettings:IDependency
    {
        public string App(params string[] sections);
    }
    public class NacosAppsettings : INacosAppsettings
    {
        private static readonly string DATAID = "";

        private static readonly string GROUP = "DEFAULT_GROUP";

        private INacosConfigClient nacosConfig { get; set; }

        public NacosAppsettings(INacosConfigClient nacosConfig)
        {
            this.nacosConfig = nacosConfig;
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public string App(params string[] sections)
        {
            try
            {
                var json = nacosConfig.GetConfigAsync(new GetConfigRequest
                {
                    DataId = DATAID,
                    Group = GROUP,
                }).Result;
                return GetValue(json, sections);
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取nacos配置异常",ex);
            }
            return null;
        }

        private string GetValue(string json, string[] sections)
        {
            var obj = JObject.Parse(json);
            for (int i = 0; i < sections.Length - 1; i++)
            {
                obj = (JObject)obj[sections[i]];
            }
            if (obj == null || obj[sections[sections.Length - 1]] == null)
                return "";//不存在

            return obj[sections[sections.Length - 1]].ToString();
        }
    }
    public class GlobalVar
    {
       // private static INacosAppsettings appsettings = AutofacContainerModule.GetService<INacosAppsettings>();
        private static INacosAppsettings appsettings = AutofacContainerModule.Resolve<INacosAppsettings>();
        ///// <summary>
        ///// 实例id
        ///// </summary>
        public static string AudioExchangeUrl = appsettings.App("AudioExchangeUrl");
    }

}
