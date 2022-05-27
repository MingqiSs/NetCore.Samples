using Infrastructure.Utilities;
using KF.ElasticSearch.Interfaces;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KF.ElasticSearch.Provider
{
    public class EsService : IEsIndexService, ITransientDependency
    {
        private readonly IElasticClient _elasticClient;

        public EsService(IEsClientService esClientService)
        {
            _elasticClient = esClientService.Client;
        }

        #region Delete

        public DeleteByQueryResponse DeleteByQuery<T>(Expression<Func<T, bool>> expression, string index = "")
            where T : class, new()
        {
            var indexName = index.GetIndex<T>();
            var request = new DeleteByQueryRequest<T>(indexName);
            var response = _elasticClient.DeleteByQuery(request);
            if (!response.IsValid)
            {
                LogHelper.Info("es_del_error:" + response.OriginalException.Message);
            }
            return response;
        }

        #endregion

        #region Update

        public IUpdateResponse<T> Update<T>(string key, T entity, string index = "") where T : class
        {
            var indexName = index.GetIndex<T>();
            var request = new UpdateRequest<T, object>(indexName, key)
            {
                Doc = entity
            };
            //request.Refresh = 0;
            var response = _elasticClient.Update(request);

            if (!response.IsValid)
            {
               // LogHelper.Info("es_update_error:" + response.OriginalException.Message);
            }
            return response;
        }

        #endregion

        #region Index

        /// <inheritdoc cref="IEsIndexService.IndexExistsAsync" />
        public bool IndexExists(string index)
        {
            var res = _elasticClient.Indices.Exists(index);
            return res.Exists;
        }

        /// <inheritdoc cref="IEsIndexService.IndexExistsAsync" />
        public async Task<bool> IndexExistsAsync(string index)
        {
            var res = await _elasticClient.Indices.ExistsAsync(index);
            return res.Exists;
        }

        public bool Insert<T>(T entity, string index = "") where T : class
        {
            var indexName = index.GetIndex<T>();
            var exists = IndexExists(indexName);
            if (!exists)
            {
                ((ElasticClient)_elasticClient).CreateIndex<T>(indexName);
                AddAlias(indexName, typeof(T).Name);
            }

            var response = _elasticClient.Index(entity,
                s => s.Index(indexName));

            if (!response.IsValid)
            {
                LogHelper.Info("es_insert_error:" + response.OriginalException.Message);
            }

            return response.IsValid;
        }

        public async Task<bool> InsertAsync<T>(T entity, string index = "", int numberOfShards = 5, int numberOfReplicas = 1, int refreshInterval = 5) where T : class
        {
            var indexName = index.GetIndex<T>();
            var exists = await IndexExistsAsync(indexName);
            if (!exists)
            {
                await ((ElasticClient)_elasticClient).CreateIndexAsync<T>(indexName, numberOfShards, numberOfReplicas, refreshInterval);
                await AddAliasAsync(indexName, typeof(T).Name);
            }

            var response = await _elasticClient.IndexAsync(entity,
                s => s.Index(indexName));

            if (!response.IsValid)
            {
                LogHelper.Info("es_insert_error:" + response.OriginalException.Message);
            }

            return response.IsValid;
        }

        public async Task InsertRangeAsync<T>(IEnumerable<T> entity, string index) where T : class
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
                LogHelper.Info("es_insertRange_error:" + response.OriginalException.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task RemoveIndex(string index)
        {
            var exists = await IndexExistsAsync(index);
            if (!exists) return;
            var response = await _elasticClient.Indices.DeleteAsync(index);

            if (!response.IsValid)
                throw new Exception("删除index失败:" + response.OriginalException.Message);
        }

        #endregion

        #region Alias

        public BulkAliasResponse AddAlias(string index, string alias)
        {
            var response = _elasticClient.Indices.BulkAlias(b => b.Add(al => al
                .Index(index)
                .Alias(alias)));

            if (!response.IsValid)
                throw new Exception("添加Alias失败:" + response.OriginalException.Message);
            return response;
        }

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

        public BulkAliasResponse RemoveAlias(string index, string alias)
        {
            var response = _elasticClient.Indices.BulkAlias(b => b.Remove(al => al
                .Index(index)
                .Alias(alias)));

            if (!response.IsValid)
                throw new Exception("删除Alias失败:" + response.OriginalException.Message);
            return response;
        }

        public BulkAliasResponse RemoveAlias<T>(string alias) where T : class
        {
            //  await _elasticClient.Sql.QueryAsync(x => "");
            return RemoveAlias(string.Empty.GetIndex<T>(), alias);
        }

        #endregion
    }
}
