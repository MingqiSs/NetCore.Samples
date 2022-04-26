using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Samples.Repository.Context;
using System;

namespace Samples.Manager.Api.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    public static class DatabaseSetup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddDatabaseSetup(this IServiceCollection services, IConfiguration configuration)
        {

            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddDbContext<BookStoreContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("BookStoreDB"));
            });

        }
    }
}
