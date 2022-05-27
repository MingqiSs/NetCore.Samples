
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SignalRDemo.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    public static class JwtSetup
    {
        /// <summary>
        /// jwt配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddJwtAuthSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            // JWT Setup
            var appSettingsSection = configuration.GetSection("TokenManagement");
            services.Configure<TokenManagement>(appSettingsSection);
            var tokenManagement = appSettingsSection.Get<TokenManagement>();
            var key = Encoding.ASCII.GetBytes(tokenManagement.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,//是否验证Issuer
                    ValidateAudience = false,//是否验证Audience
                    ValidateLifetime = false,//是否验证失效时间
                    ValidateIssuerSigningKey = false,//是否验证SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(key),//拿到SecurityKey
                    ValidAudience = tokenManagement.Audience,
                    ValidIssuer = tokenManagement.Issuer

                };
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Path.StartsWithSegments("/chathub"))
                        {
                            string accesToken = context.Request.Query["access_token"];
                            if (!string.IsNullOrEmpty(accesToken))
                            {
                                context.HttpContext.Request.Headers.Add("Authorization", new StringValues(accesToken));
                                //context.HttpContext.User = new ClaimsPrincipal();
                            }
                        }
                        return Task.CompletedTask;
                    },
                     OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.Clear();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 401;
                        context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = "授权未通过", status = false, code = 401 }));
                        return Task.CompletedTask;
                    }
                };
            });

        }
    }
}
