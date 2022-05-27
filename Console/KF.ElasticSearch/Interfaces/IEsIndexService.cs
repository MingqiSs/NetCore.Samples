using Nest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KF.ElasticSearch.Interfaces
{
    public interface IEsIndexService
    {
        /// <summary>
        ///     是否存在
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        Task<bool> IndexExistsAsync(string index);

        bool Insert<T>(T entity, string index = "") where T : class;

        /// <summary>
        ///新增数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="index"></param>
        Task<bool> InsertAsync<T>(T entity, string index = "", int numberOfShards = 5, int numberOfReplicas = 1, int refreshInterval = 5) where T : class;

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="index"></param>
        Task InsertRangeAsync<T>(IEnumerable<T> entity, string index = "") where T : class;
        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        Task RemoveIndex(string index);

        #region Delete
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        DeleteByQueryResponse DeleteByQuery<T>(Expression<Func<T, bool>> expression, string index = "") where T : class, new();
        #endregion

        #region Update
        IUpdateResponse<T> Update<T>(string key, T entity, string index = "") where T : class;
        #endregion

        #region 别名

        /// <summary>
        ///     添加别名
        /// </summary>
        /// <param name="index"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        Task<BulkAliasResponse> AddAliasAsync(string index, string alias);

        Task<BulkAliasResponse> AddAliasAsync<T>(string alias) where T : class;

        /// <summary>
        ///     删除别名
        /// </summary>
        /// <param name="index"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        BulkAliasResponse RemoveAlias(string index, string alias);

        BulkAliasResponse RemoveAlias<T>(string alias) where T : class;

        //別名滚动
        //说明：
        //如果别名logs_write指向的索引是7天前（含）创建的或索引的文档数>=1000或索引的大小>= 5gb，则会创建一个新索引 logs-000002，并把别名logs_writer指向新创建的logs-000002索引
        //注意：rollover是你请求它才会进行操作，并不是自动在后台进行的。你可以周期性地去请求它。
        //await elasticClient.Indices.RolloverAsync("ab", x => x.Conditions(m => m.MaxAge(TimeSpan.FromDays(7)).MaxDocs(1000)));
        #endregion
    }
}
