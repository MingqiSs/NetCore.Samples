using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Samples.Manager.Api.Filter
{
    /// <summary>
    /// 记录接口请求响应时间
    /// </summary>
    public class TimmerFilter : IActionFilter
    {
        private readonly ILogger _logger;
        private readonly Stopwatch Timmer = new Stopwatch();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public TimmerFilter(ILogger<TimmerFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 在操作执行之前调用动作，在动作结果之前。
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {

            Timmer.Stop();
            var useTime = Timmer.ElapsedMilliseconds;
            var s = useTime / 1000.0;
            // 时间超过2秒记录下来
            if (s > 2)
            {
                _logger.LogInformation($"请求接口：{context.HttpContext.Request.Host}{context.HttpContext.Request.Path.Value}，请求方式：{context.HttpContext.Request.Method}，执行完成时间{s}秒");
            }
        }

        /// <summary>
        /// 在执行操作之前调用，在模型绑定完成之后。
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Timmer.Start();
        }

    }
}
