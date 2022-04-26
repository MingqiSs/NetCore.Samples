using Infrastructur.AutofacManager;
using Samples.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Service.Domain.BaseProvider
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseSerivceDomin<T, TRepository> : IDependency
         where TRepository : IBookStoreRepository<T>
         where T : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        protected IBookStoreRepository<T> repository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public BaseSerivceDomin(IBookStoreRepository<T> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// 通用删除
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        //public virtual WebResponseContent Del(KeyOptions options)
        //{
        //    Type entityType = typeof(T);
        //    var tableName = entityType.Name;
        //    var keyProperty = entityType.GetKeyProperty();
        //    if (keyProperty == null || options == null || options.Key.Count == 0) return Response.Error(ResponseType.KeyError);
        //    //查找key
        //    var tKey = keyProperty.Name;
        //    if (string.IsNullOrEmpty(tKey))
        //        return Response;
        //    //判断条件
        //    FieldType fieldType = entityType.GetFieldType();
        //    string joinKeys = (fieldType == FieldType.Int || fieldType == FieldType.BigInt)
        //         ? string.Join(",", options.Key)
        //         : $"'{string.Join("','", options.Key)}'";
        //    //逻辑删除
        //    string sql = $"update {tableName} set Enable=2 where {tKey} in ({joinKeys});";
        //    Response.Status = repository.Sql_ExecuteCommand(sql) > 0;
        //    if (Response.Status && string.IsNullOrEmpty(Response.Message)) Response.OK(ResponseType.DelSuccess);
        //    return Response;
        //}
    }
}
