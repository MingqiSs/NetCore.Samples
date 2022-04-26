using Hangfire.Server;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Hangfire
{
    public class ServerExceptionHangfireFilter : IServerExceptionFilter
    {
        ILogger _logger;

        public ServerExceptionHangfireFilter(ILogger<ServerExceptionHangfireFilter> logger)
        {
            _logger = logger;
        }

        public void OnServerException(ServerExceptionContext filterContext)
        {
            _logger.LogError(filterContext.Exception?.Message, filterContext.Exception);
            filterContext.ExceptionHandled = true;
        }
    }
}
