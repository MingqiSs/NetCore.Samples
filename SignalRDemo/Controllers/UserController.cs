using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SignalRDemo.Token;

namespace SignalRDemo.Controllers
{

    [Authorize]
    public class UserController : Controller
    {
        private readonly TokenManagement _tokenManagement;
        public UserController(IOptions<TokenManagement> tokenManagement)
        {
            _tokenManagement = tokenManagement.Value;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("User/Login")]
        public IActionResult Login()
        {
            var userToken = new UserToken
            {
                IP = "",
                IMEI = "",
                Channel = "",
                Version = "",
                UID ="123",
                Name = "张三",
                Email ="",
                Mobile = "",
                MobileArea = "",
                Account =1,
            };

            string token = userToken.Serialization(_tokenManagement);
            HttpContext.Response.Headers.Add("Authorization", new StringValues(token));
            return Ok(token);
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("User/GetUserInfo")]
        public IActionResult GetUserInfo()
        {
            var userToken = new UserToken
            {
                IP = "",
                IMEI = "",
                Channel = "",
                Version = "",
                UID = "123",
                Name = "张三",
                Email = "",
                Mobile = "",
                MobileArea = "",
                Account = 1,
            };
            return Ok(userToken);
        }
    }
}
