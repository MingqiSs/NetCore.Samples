using Infrastructure.Config;
using Infrastructure.Models;
using Infrastructure.UserManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Samples.Manager.Api.Filter;
using Samples.Service.APP.Dto;
using Samples.Service.APP.Interface;
using Samples.Service.APP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Samples.Manager.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [JwtAuthorizeAttribute]
    [Route("v1")]
    [ApiController]
    public class UserController : ControllerBase
    {
      
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly TokenManagement _tokenManagement;
       /// <summary>
       /// 
       /// </summary>
       /// <param name="logger"></param>
       /// <param name="userService"></param>
       /// <param name="tokenManagement"></param>
        public UserController(ILogger<UserController> logger, IUserService userService, IOptions<TokenManagement> tokenManagement)
        {
            _logger = logger;
            _userService = userService;
            _tokenManagement = tokenManagement.Value;
        }
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost, Route("W100")]
        [ProducesResponseType(typeof(ResultDto<bool>), 200)]
        public async Task<IActionResult> SendValidCodeAsync(ValidCodeRQ rq)
        {
            var result = await _userService.SendValidCodeAsync(rq);
            return Ok(result);
        }
        /// <summary>
        ///注册
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("W101"), AllowAnonymous]
        [ProducesResponseType(typeof(UserInfoRP), 200)]
        public async Task<IActionResult> RegisterAsync(RegisterRQ rq)
        {
            var r = _userService.Register(rq);

            if (r.Res == 1 && r.Dt != null && !string.IsNullOrEmpty(r.Dt.Uid))
            {
                var userinfo = await _userService.GetUserInfoAsync(r.Dt.Uid);
                if (r.Res == 1 && userinfo.Dt.IsLogin)
                {
                    AddAuthorization(userinfo.Dt);

                    return Ok(userinfo);
                }
            }
            return Ok(r);
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        [HttpPost, Route("W102"), AllowAnonymous]
        [ProducesResponseType(typeof(UserInfoRP), 200)]
        public async Task<IActionResult> LoginAsync(UserLoginRQ rq)
        {
            var r = await _userService.LoginAsync(rq);
            if (r.Res != 1) return Ok(r);
            AddAuthorization(r.Dt);
            return Ok(r);
        }
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        [HttpPost, Route("W103")]
        [ProducesResponseType(typeof(UserInfoRP), 200)]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            var uid = UserContext.Current.UserInfo.UID;
            var userinfo = await _userService.GetUserInfoAsync(uid);

            return Ok(userinfo);
        }      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        private void AddAuthorization(UserInfoRP model)
        {
            var userToken = new UserToken
            {
                IP = "",
                IMEI = "",
                Channel = "",
                Version = "",
                UID = model.UID,
                Name = model.Name,
                Email = model.Email,
                Mobile=model.Mobile,
                MobileArea = model.AreaCode,
                Account = model.Account,
            };

            string token = userToken.Serialization(_tokenManagement);
            HttpContext.Response.Headers.Add("Authorization", new StringValues(token));
        }

    }
}
