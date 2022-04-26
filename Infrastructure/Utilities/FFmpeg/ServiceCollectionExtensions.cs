using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Utilities.FFmpeg
{
    public static partial class ServiceCollectionExtensions
    {
        public static void AddFFmpeg(this IServiceCollection services)
        {
            IFFmpeg ffmpeg;
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    ffmpeg = new LinuxFFmpeg();
                    break;
                case PlatformID.Win32NT:
                    ffmpeg = new WindowsFFmpeg();
                    break;
                default:
                    ffmpeg = new WindowsFFmpeg();
                    break;
                    //   throw new PlatformNotSupportedException("不支持的系统");
            }
            services.AddSingleton(ffmpeg);
        }
    }
}
