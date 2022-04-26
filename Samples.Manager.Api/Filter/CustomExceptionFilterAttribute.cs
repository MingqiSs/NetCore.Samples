using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Samples.Manager.Api.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _log;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> log)
        {
            _log = log;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            var request = context.HttpContext.Request;
            var pars = request.QueryString.ToString();
            try
            {
                if (request.Method.ToLower() == "post")
                {
                    request.Body.Seek(0, SeekOrigin.Begin);
                    using (var sr = new StreamReader(request.Body))
                    {
                        pars = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex) { _log.LogError($"方法(OnException)，请求中出现异常,详情为：{ex.Message}"); }
            var fromData = @"Path:{0},输入参数:{1},IP:{2}";
            fromData = string.Format(@fromData, request.Path.ToString(), pars, request.Host);

            var log = $"{fromData}--->{DateTime.Now.ToString("yyyy - MM - dd HH: mm: ss")}——{context.Exception.GetType().ToString()}：{context.Exception.Message}——异常堆栈信息：{context.Exception.StackTrace}";
            _log.LogError(context.Exception, log);

            context.Result = new JsonResult(new {
                Res = 0,
                Msg = "网络请求超时,请稍后再试 !",
                Dt = false
            });
        }

    }
}
