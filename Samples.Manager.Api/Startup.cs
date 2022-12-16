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
using Infrastructure.Config;
using AutoMapper;
using Samples.Service.APP.AutoMapper;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
           // services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            services.AddMvc().AddMvcOptions(options =>
            {
                options.Filters.Add(typeof(TimmerFilter));
              //  options.Filters.Add(typeof(CustomExceptionFilterAttribute));
                options.Filters.Add(typeof(AuthActionFilter)); 
            });
           
            services.AddHttpContextAccessor();

            #region Nacos服务发现,配置中心
            services.AddHttpClient();

            var addList = new List<string>
            {
               ""
            };
            var dic = new Dictionary<string, string>
            {
                { "version", "V1.0.0" }
            };
            services.AddNacosAspNetCore(
            options =>
            {
                options.ServiceName = "nacos-service-demo";//注册服务名
                options.ServerAddresses = addList;
                options.Weight = 100;
                options.DefaultTimeOut = 15000;
                options.Metadata = dic;
                options.Port = 9898;
            }, nacosOption =>
            {
                nacosOption.DefaultTimeOut = 15000;
                nacosOption.ServerAddresses = addList;
                nacosOption.ListenInterval = 5000;
            });
            // services.AddHostedService<ListenConfigurationBgTask>();
            #endregion


            services.AddControllers();

            services.AddSwaggerSetup();

            services.AddJwtAuthSetup(Configuration);

            services.AddDatabaseSetup(Configuration);

            services.AddAutofac();
            services.AddHttpClient<IHttpClientService, HttpClientService>();

            services.AddAutoMapper(typeof(DomainToViewModelMappingProfile), typeof(ViewModelToDomainMappingProfile));
            services.AddCors(option => option.AddPolicy("samples", policy => policy.AllowAnyHeader()
                                                                                 .AllowAnyMethod()
                                                                                 .AllowCredentials()
                                                                                 .WithOrigins()
                                                                                 ));
            #region 注册 日志
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

            //静态资源
            app.UseStaticFiles();

            //配置HttpContext
            app.UseStaticHttpContext();

            app.UseSwaggerSetup();

            // 跨域配置
            app.UseCors();

            //请求错误提示配置
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
        /// 配置HttpContext
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
