using Elasticsearch.Net;
using KF.ElasticSearch.Config;
using KF.ElasticSearch.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KF.ElasticSearch
{
    public class EsClientService : IEsClientService, ITransientDependency
    {
        public ElasticClient Client { get; }

        public EsClientService(IOptions<EsConfig> esConfig,
            ILogger<EsClientService> logger)
        {
            try
            {
                var uris = esConfig.Value.Uris;
                if (uris == null || uris.Count < 1)
                {
                    throw new Exception("urls can not be null");
                }

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

                if (!string.IsNullOrWhiteSpace(esConfig.Value.UserName) && !string.IsNullOrWhiteSpace(esConfig.Value.Password))
                    connectionSetting.BasicAuthentication(esConfig.Value.UserName, esConfig.Value.Password);

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
