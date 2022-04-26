using Hangfire;
using Hangfire.Dashboard;
using Hangfire.RecurringJobExtensions;
using Infrastructure.Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;


namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public static class HangfireApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHangfireCore(this IApplicationBuilder app, 
            List<IDashboardAuthorizationFilter> authFilters = null,
            string jsonFile="")
        {
            var sp = app.ApplicationServices;
            var conf = sp.GetService<IGlobalConfiguration>();
            var exFilter = sp.GetService<ServerExceptionHangfireFilter>();
            var notReentryFilter = sp.GetService<NotReentryServerHangfireFilter>();

            conf.UseRecurringJob(new JobProvider(jsonFile))
                .UseFilter(exFilter)
                .UseFilter(notReentryFilter);

            app.UseHangfireDashboard(pathMatch: "/jobs", options: new DashboardOptions
            {
                Authorization = authFilters ?? new List<IDashboardAuthorizationFilter>()
            });

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                ServerName = IPHostHelper.GetIpV4s().FirstOrDefault(),
                Queues = new string[] { "default", "system", "jobs" }
            });

            return app;
        }

    }
}
