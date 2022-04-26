using Infrastructure.Models;
using Samples.Service.APP.Dto;
using Samples.Service.APP.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Samples.Service.APP.Dto.RegisterRQ;

namespace Samples.Service.APP.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        Task<ResultDto<bool>> SendValidCodeAsync(ValidCodeRQ rq);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        ResultDto<RegisterRP> Register(RegisterRQ rq);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Task<ResultDto<UserInfoRP>> GetUserInfoAsync(string uid);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        Task<ResultDto<UserInfoRP>> LoginAsync(UserLoginRQ rq);
    }
}
