using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Samples.Manager.Api.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.AutofacManager;
using NLog.Extensions.Logging;
using Samples.Manager.Api.Filter;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Samples.Service.APP.Interface;
using Samples.Service.APP.Common;


namespace Samples.Manager.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 
        /// </summary>
       // public ILifetimeScope AutofacContainer { get; private set; }
        public IConfiguration Configuration { get; }

        #region net core 2.1
        //public IServiceProvider ConfigureServices(IServiceCollection services)
        //{
        //    services.AddControllers();
        //    AutoMapper Settings
        //    services.AddAutoMapperSetup();
        //    Add Autofac
        //    var builder = new ContainerBuilder();
        //    builder.RegisterModule();

        //    builder.Populate(services);
        //    AutofacContainer = builder.Build();
        //    return new AutofacServiceProvider(AutofacContainer);
        //}
        #endregion
        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
         
            services.AddMvc().AddMvcOptions(options =>
            {
                options.Filters.Add(typeof(TimmerFilter));
                options.Filters.Add(typeof(CustomExceptionFilterAttribute));
                options.Filters.Add(typeof(AuthActionFilter)); 
            });
           
            services.AddHttpContextAccessor();

            services.AddControllers();

            services.AddSwaggerSetup();

            services.AddJwtAuthSetup(Configuration);

            services.AddDatabaseSetup(Configuration);

            services.AddAutofac();

            services.AddHttpClient<IHttpClientService, HttpClientService>();

            //services.AddCors(option => option.AddPolicy("samples", policy => policy.AllowAnyHeader()
            //                                                                     .AllowAnyMethod()
            //                                                                     .AllowCredentials()
            //                                                                     .WithOrigins()
            //                                                                     ));
            #region ◊¢≤· »’÷æ
            services.AddLogging(t => t.AddNLog());
            #endregion
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //æ≤Ã¨◊ ‘¥
            app.UseStaticFiles();

            //≈‰÷√HttpContext
            app.UseStaticHttpContext();

            app.UseSwaggerSetup();

            // øÁ”Ú≈‰÷√
            app.UseCors();

            //«Î«Û¥ÌŒÛÃ· æ≈‰÷√
            //app.UseErrorHandling();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public static class StaticHttpContextExtensions
    {
        /// <summary>
        /// ≈‰÷√HttpContext
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            Infrastructure.Utilities.HttpContext.Configure(httpContextAccessor);
            return app;
        }
    }
}
