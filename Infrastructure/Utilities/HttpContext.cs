﻿using Infrastructure.Enums;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Utilities
{
    public static class HttpContext
    {
        private static IHttpContextAccessor _accessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => _accessor.HttpContext;

        public static void Configure(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        /// <summary>
        /// IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIp
        {
            get
            {
                var ip = _accessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (string.IsNullOrEmpty(ip))
                {
                    ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString().Replace("::1", "127.0.0.1");
                }
                return ip;
            }
        }
        /// <summary>
        /// 获取平台类型
        /// </summary>
        public static EnumPlatformType GetPlatform
        {
            get
            {
                var key = "Platform";
                var header = _accessor.HttpContext.Request.Headers[key];
                if (header.Count == 0) return EnumPlatformType.WAP;

                return header.ToString().ToEnum(EnumPlatformType.WAP);
            }
        }
        /// <summary>
        /// 获取版本号
        /// </summary>
        public static string GetVersion
        {
            get
            {
                var key = "Version";
                var header = _accessor.HttpContext.Request.Headers[key];
                if (header.Count == 0) return string.Empty;
                return header.ToString();
            }
        }
        /// <summary>
        /// 获取渠道
        /// </summary>
        public static string GetChannel
        {
            get
            {
                var key = "Channel";
                var header = _accessor.HttpContext.Request.Headers[key];
                if (header.Count == 0) return string.Empty;
                return header.ToString();
            }
        }
        /// <summary>
        /// IMEI
        /// </summary>
        public static string GetIMEI
        {
            get
            {
                try
                {
                    var imei = _accessor.HttpContext.Request.Headers["IMEI"].FirstOrDefault();
                    return imei;
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }
    }
}
