using Infrastructure.CacheManager;
using Infrastructure.Config;
using Infrastructure.Models;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nacos;
using Samples.Service.APP.AutoMapper;
using Samples.Service.APP.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Samples.Manager.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("v1")]
    public class NacosController : ControllerBase
    {

        private readonly ILogger<NacosController> _logger;

        private readonly INacosConfigClient _nacosConfigClient;
        private readonly IHttpClientService _httpClientService;
        private readonly IUserService _userService;
        private readonly ICommonService _commonService;
       /// <summary>
       /// 
       /// </summary>
       /// <param name="logger"></param>
       /// <param name="nacosConfigClient"></param>
       /// <param name="httpClientService"></param>
       /// <param name="userService"></param>
       /// <param name="commonService"></param>
        public NacosController(ILogger<NacosController> logger, 
            INacosConfigClient nacosConfigClient, 
            IHttpClientService httpClientService,
            IUserService userService,
            ICommonService commonService)
        {
            _logger = logger;
            _nacosConfigClient = nacosConfigClient;
            _httpClientService = httpClientService;
            _userService = userService;
            _commonService = commonService;
        }
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet, Route("W200")]
        [ProducesResponseType(typeof(ResultDto<bool>), 200)]
        public async Task<string> Get(string key)
        {
            LogHelper.Info("test");
            var msg = "";
            var f = string.IsNullOrEmpty(msg) ? "请求成功！" : msg;
            var d = GlobalVar.AudioExchangeUrl;
            var res = await _nacosConfigClient.GetConfigAsync(new GetConfigRequest
            {
                DataId = key,
                Group = "DEFAULT_GROUP",
            });
        
            return  res;
        }    
    }
}
