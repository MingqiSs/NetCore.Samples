using Infrastructure.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Samples.Manager.Api.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly RequestDelegate next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loggerFactory"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, ILoggerFactory loggerFactory)
        {
            ILogger<ErrorHandlingMiddleware> _logger = loggerFactory.CreateLogger<ErrorHandlingMiddleware>();

            var request = context.Request;
            var pars = request.QueryString.ToString();
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                var statusCode = context.Response.StatusCode;

                var msg = string.Empty;
                if (statusCode == 401)
                {
                    var fromData = @"Path:{0},输入参数:{1},IP:{2},Platform:{3},Version:{4},ToKen:{5}";

                    var platform = request.Headers["Platform"];

                    var version = request.Headers["Version"];

                    var authorizationHeader = request.Headers["Authorization"];

                    if (!string.IsNullOrWhiteSpace(authorizationHeader))
                    {
                        msg = string.Format(@fromData, request.Path.ToString(), pars, request.Host, platform, version, authorizationHeader);
                    }

                }
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    _logger.LogError($"异常编码：{statusCode},{msg}");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="returnCode"></param>
        /// <param name="statusCode"></param>
        /// <param name="msg"></param>
        private static void HandleException(HttpContext context, ResponseCode returnCode, int statusCode, string msg)
        {
            var data = new { res = 0, ec = returnCode, msg = msg, dt = false };
            var result = JsonConvert.SerializeObject(data);

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json;charset=utf-8";
            context.Response.WriteAsync(result, Encoding.UTF8);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ErrorHandlingExtensions
    {
        /// <summary>
        /// 注入错误处理Middleware
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }

}
