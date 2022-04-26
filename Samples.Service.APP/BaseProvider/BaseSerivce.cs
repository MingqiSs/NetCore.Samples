using Infrastructur.AutofacManager;
using Infrastructure.Enums;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Service.APP.BaseProvider
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseSerivce : IDependency
    {
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
    }
}
