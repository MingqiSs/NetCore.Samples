using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRDemo.Middleware
{
    public class RequestSignCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestSignCheckMiddleware> _logger;

        public RequestSignCheckMiddleware(RequestDelegate next, ILogger<RequestSignCheckMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        /// <summary>
        /// 权限认证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments("/chathub"))
            {
                //授权认证逻辑
                await _next.Invoke(context);
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
    public static class SignCheckExtensions
    {
        public static IApplicationBuilder UseRequestSignCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestSignCheckMiddleware>();
        }
    }
}
