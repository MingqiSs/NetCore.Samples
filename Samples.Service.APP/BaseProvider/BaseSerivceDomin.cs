using Infrastructur.AutofacManager;
using Infrastructure.Enums;
using Infrastructure.Models;
using Samples.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Service.APP.BaseProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TRepository"></typeparam>
    public class BaseSerivceDomin<TEntity, TRepository> : IDependency
      where TRepository : IBookStoreRepository<TEntity>
      where TEntity : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        protected TRepository repository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public BaseSerivceDomin(TRepository repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inData"></param>
        /// <returns></returns>
        public ResultDto<T> Result<T>(T inData)
        {
            return new ResultDto<T>(inData);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="inData"></param>
        /// <returns></returns>
        public ResultDto<T> Result<T>(T inData, string msg)
        {
            return new ResultDto<T>(inData) { Msg = msg };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inData"></param>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ResultDto<T> Result<T>(T inData, ResponseCode code, string msg)
        {
            return new ResultDto<T>(inData) { Ec = code, Msg = msg };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public ResultDto<T> Result<T>(ResponseCode code, string errorMsg)
        {
            return new ResultDto<T>(code, errorMsg);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="errorMsg">errorMsg</param>
        /// <returns></returns>
        public ResultDto<T> Result<T>(string errorMsg = "")
        {
            return new ResultDto<T>(ResponseCode.sys_exception, errorMsg);
        }
        ///// <summary>
        ///// 通用删除
        ///// </summary>
        ///// <param name="options"></param>
        ///// <returns></returns>
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
