using KF.ElasticSearch.Config;
using KF.ElasticSearch.Interfaces;
using KF.ElasticSearch.Provider;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace KF.ElasticSearch.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param> 
        /// <param name="setupAction"></param>
        public static void AddEsService(this IServiceCollection services, Action<EsConfig> setupAction)
        {
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction), "调用 Elasticsearch 配置时出错，未传入配置信息。");

            services.Configure(setupAction);

            services.AddTransient<IEsClientService, EsClientService>();
            services.AddTransient<IEsIndexService, EsService>();
            services.AddTransient<IEsSearchService, EsSearchService>();
        }
    }
}
