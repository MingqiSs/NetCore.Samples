using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Samples.Service.APP.AutoMapper;
using Samples.Service.APP.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Samples.Manager.Api.Controllers
{
    [Route("v1")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IHttpClientService _httpClientService;
        private readonly ICommonService _commonService;
       private readonly ITestService _testService;
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="logger"></param>
        ///// <param name="httpClientService"></param>
        ///// <param name="commonService"></param>
        ///// <param name="testService"></param>
        public TestController(ILogger<TestController> logger,

            IHttpClientService httpClientService,
            ICommonService commonService,
            ITestService testService)
        {
            _logger = logger;
            _httpClientService = httpClientService;
            _commonService = commonService;
              _testService = testService;
        }
        /// <summary>
        /// SendTestAsync
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        [HttpPost, Route("W301")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> SendTestAsync()
        {
            var data = await _httpClientService.PostAsync("http://localhost:19030", "/v1/W303", String.Empty);

            return Ok(data);
        }

        /// <summary>
        /// 測試TestAsync
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        [HttpPost, Route("W303")]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult GetTestAsync()
        {
            Thread.Sleep(50000);
            //  throw new Exception();
            // _commonService.PdfDemo();
            return Ok("ok");
        }
        /// <summary>
         /// 测试AutoMapper
         /// </summary>
        [HttpPost, Route("W304")]
        [ProducesResponseType(typeof(string), 200)]
        public void TestAutoMapper()
        {
            _testService.TestAutoMapper(new ChattingRetryAddCustomerParamDto { });
        }
        /// <summary>
        /// 测试TestAutowired
        /// </summary>
        [HttpPost, Route("W305")]
        [ProducesResponseType(typeof(string), 200)]
        public void TestAutowired()
        {
            _testService.TestAutowired();
        }
    }
}
