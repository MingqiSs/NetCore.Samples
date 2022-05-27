using KF.ElasticSearch.Config;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KF.ElasticSearch.Interfaces;
using Infrastructure.Config;

namespace KF.ElasticSearch.Provider
{
    public class EsClientProvider : IEsClientProvider, ITransientDependency
    {
        public ElasticClient Client { get; }

        public EsClientProvider(
            ILogger<EsClientProvider> logger)
        {
            try
            {
                var configUri = AppSetting.GetConfig("ElasticSearch:Url") ;

                var userName = AppSetting.GetConfig("ElasticSearch:UserName") ;
                var password = AppSetting.GetConfig("ElasticSearch:Password");

                if (configUri == null)
                {
                    throw new Exception("urls can not be null");
                }

                List<Uri> uris = configUri.Split(',').Select(x => new Uri(x)).ToList();

                ConnectionSettings connectionSetting;
                if (uris.Count == 1)
                {
                    var uri = uris.First();
                    connectionSetting = new ConnectionSettings(uri);
                }
                else
                {
                    var connectionPool = new SniffingConnectionPool(uris);
                    connectionSetting = new ConnectionSettings(connectionPool).DefaultIndex("");
                }

                if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
                    connectionSetting.BasicAuthentication(userName, password);

                Client = new ElasticClient(connectionSetting);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ElasticSearch Initialized failed.");
                throw;
            }
        }
    }
}
