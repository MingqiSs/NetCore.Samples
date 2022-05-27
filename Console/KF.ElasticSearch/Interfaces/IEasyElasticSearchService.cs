using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KF.ElasticSearch.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEasyElasticSearchService
    {
        /// <summary>
        ///是否存在
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        Task<bool> IndexExistsAsync(string index);

        /// <summary>
        ///新增数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="index"></param>
        Task<int> InsertAsync<T>(T entity, string index = "") where T : class;

        /// <summary>
        ///批量新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="index"></param>
        Task<int> InsertRangeAsync<T>(IEnumerable<T> entity, string index = "") where T : class;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        Task<int> RemoveIndex(string index);
        /// <summary>
        ///     添加别名
        /// </summary>
        /// <param name="index"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        Task<BulkAliasResponse> AddAliasAsync(string index, string alias);

        Task<BulkAliasResponse> AddAliasAsync<T>(string alias) where T : class;
    }
}
