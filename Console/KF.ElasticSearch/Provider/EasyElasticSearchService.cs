using KF.ElasticSearch.Interfaces;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.ElasticSearch.Provider
{
    public class EasyElasticSearchService : IEasyElasticSearchService, ITransientDependency
    {
        private readonly IElasticClient _elasticClient;

        public EasyElasticSearchService(IEsClientProvider esClientProvider)
        {
            _elasticClient = esClientProvider.Client;
        }
        #region Alias
        public async Task<BulkAliasResponse> AddAliasAsync(string index, string alias)
        {
            var response = await _elasticClient.Indices.BulkAliasAsync(b => b.Add(al => al
                .Index(index)
                .Alias(alias)));

            if (!response.IsValid)
                throw new Exception("添加Alias失败:" + response.OriginalException.Message);
            return response;
        }

        public async Task<BulkAliasResponse> AddAliasAsync<T>(string alias) where T : class
        {
            return await AddAliasAsync(string.Empty.GetIndex<T>(), alias);
        }
        #endregion
        /// <summary>
        /// 是否存在索引
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<bool> IndexExistsAsync(string index)
        {
            var res = await _elasticClient.Indices.ExistsAsync(index);
            return res.Exists;
        }
        #region  新增数据
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync<T>(T entity, string index = "") where T : class
        {
            var indexName = index.GetIndex<T>();
            var exists = await IndexExistsAsync(indexName);
            if (!exists)
            {
                await ((ElasticClient)_elasticClient).CreateIndexAsync<T>(indexName);
                await AddAliasAsync(indexName, typeof(T).Name);
            }

            var response = await _elasticClient.IndexAsync(entity,
                s => s.Index(indexName));

            if (!response.IsValid)
            {

               // LogHelper.Info("新增数据失败:" + response.OriginalException.Message);

                return -1;
            }

            return 0;
        }
        /// <summary>
        /// 批量新增数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync<T>(IEnumerable<T> entity, string index = "") where T : class
        {
            var indexName = index.GetIndex<T>();
            var exists = await IndexExistsAsync(indexName);
            if (!exists)
            {
                await ((ElasticClient)_elasticClient).CreateIndexAsync<T>(indexName);
                await AddAliasAsync(indexName, typeof(T).Name);
            }

            var bulkRequest = new BulkRequest(indexName)
            {
                Operations = new List<IBulkOperation>()
            };
            var operations = entity.Select(o => new BulkIndexOperation<T>(o)).Cast<IBulkOperation>().ToList();
            bulkRequest.Operations = operations;
            var response = await _elasticClient.BulkAsync(bulkRequest);

            if (!response.IsValid)
            {
                //LogHelper.Info("批量新增数据失败:" + response.OriginalException.Message);

                return -1;
            }
            return 0;
        }
        #endregion
        /// <summary>
        /// 移除索引
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<int> RemoveIndex(string index)
        {
            var exists = await IndexExistsAsync(index);
            if (!exists) return -1;
            var response = await _elasticClient.Indices.DeleteAsync(index);

            if (!response.IsValid)
            {
                //LogHelper.Info("删除index失败:" + response.OriginalException.Message);
                return -1;
            }
            return 0;

        }
    }
}
