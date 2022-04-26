using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace Infrastructure.Hangfire
{
    /// <summary>
    /// 每个job都不可重入过滤器
    /// </summary>
    public class NotReentryServerHangfireFilter : IServerFilter
    {
        /// <summary>
        /// 判断job是否正在运行
        /// </summary>
        static ConcurrentDictionary<string, DateTime> JobRunnings = new ConcurrentDictionary<string, DateTime>();

        ILogger _logger;
        public NotReentryServerHangfireFilter(ILogger<NotReentryServerHangfireFilter> logger)
        {
            _logger = logger;
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            var jobId = BuildJobId(filterContext.BackgroundJob);
            if(!JobRunnings.TryAdd(jobId, DateTime.Now))
            {
                filterContext.Canceled = true;
                return;
            }
            _logger.LogInformation($"{jobId} starting...");
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            var jobId = BuildJobId(filterContext.BackgroundJob);
            JobRunnings.TryRemove(jobId, out var tmp);
            _logger.LogInformation($"{jobId} finished.");
        }

        public string BuildJobId(BackgroundJob job)
        {
            return $"{job.Job.Type.FullName}.{job.Job.Method.Name}";
        }
    }
}
