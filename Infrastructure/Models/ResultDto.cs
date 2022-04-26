using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Enums;
namespace Infrastructure.Models
{
    /// <summary>
    /// 返回结果集（为了跟其他项目对接，统一为小写） 版本 : v.1.4
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultDto<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public int Res { get; set; }

        /// <summary>
        /// 返回码
        /// </summary>
        public ResponseCode Ec { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public T Dt { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Msg { get; set; }

        public ResultDto()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public ResultDto(T dt)
        {
            Res = 1;
            Ec = (int)ResponseCode.sys_success;
            Msg = "";
            Dt = dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        public ResultDto(ResponseCode code, string msg)
        {
            Res = 0;
            Ec = code;
            Msg = msg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="res"></param>
        /// <param name="ec"></param>
        /// <param name="msg"></param>
        public ResultDto(int res, ResponseCode ec, string msg)
        {
            Res = res;
            Ec = ec;
            Msg = msg;
        }

    }
}
