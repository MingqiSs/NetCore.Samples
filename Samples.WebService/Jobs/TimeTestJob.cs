using Hangfire.RecurringJobExtensions;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace Samples.WebService.Jobs
{
    public class TimeTestJob
    {
        private readonly ILogger<TimeTestJob> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applyAccountService"></param>
        public TimeTestJob(ILogger<TimeTestJob> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        [RecurringJob("*/5 * * * *", "jobs"),]
        [DisplayName("TestJob")]
        public void TestJob(PerformContext context)
        {

        }
    }
}
