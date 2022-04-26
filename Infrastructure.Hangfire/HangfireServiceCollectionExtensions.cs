using Hangfire;
using Hangfire.Dashboard.Resources;
using Hangfire.MemoryStorage;
using Infrastructure.Hangfire;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class HangfireServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="jobAssemblys"></param>
        /// <param name="jobSuffix"></param>
        /// <returns></returns>
        public static IServiceCollection AddHangfireCore(this IServiceCollection services, List<Assembly> jobAssemblys, string jobSuffix = "Job")
        {
            Strings.Culture = new System.Globalization.CultureInfo("zh-CN");
            services.AddTransient<ServerExceptionHangfireFilter>();
            services.AddTransient<NotReentryServerHangfireFilter>();
            services.Scan(scan => scan
                .FromAssemblies(jobAssemblys)
                    .AddClasses(classes => classes.Where(q => q.Name.EndsWith(jobSuffix)))
                        .AsSelf()
                        .WithTransientLifetime());
            services.AddHangfire(conf => conf.UseMemoryStorage());

            return services;
        }


    }
}