using Samples.Service.APP.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Service.APP.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommonService
    {
        /// <summary>
        /// 检验验证码
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="areaCode"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        bool CheckValidationCode(string name, string code, string areaCode, out string errmsg);
    }
}
