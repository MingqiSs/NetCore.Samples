using FFmpeg.NET;
using Infrastructure.Const;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utilities.FFmpeg
{
    public abstract class FFmpegBase : IFFmpeg
    {
        /// <summary>
        /// FFmpeg命令路径
        /// </summary>
        public abstract string FFmpegPath { get; }

        /// <summary>
        /// 系统命令行路径
        /// </summary>
        public abstract string CmdName { get; }

        /// <summary>
        /// FFmpeg命令参数
        /// </summary>
        public abstract string Arguments { get; }
        /// <summary>
        /// 原始方法
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public virtual string Convert(string inputPath, string outputPath)
        {
            // -i表示输入文件；-y表示强制转换，覆盖已有文件
            // 更多命令见官方文档
            var cmd = $@"{FFmpegPath} -i {inputPath} -b:v 1024k   -y {outputPath}";
            //var cmd = $@"{FFmpegPath} -i {inputPath} -b:v 1024k -preset ultrafast  -y {outputPath}";
            return Run(cmd);
        }
        /// <summary>
        /// 使用FFmpegNet组件
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        /// <param name="cutMediaTimeSpan"></param>
        /// <returns></returns>
        public virtual async Task ConvertByFFmpegNetAsync(string inputPath, string outputPath, int cutMediaTimeSpan)
        {
            var inputFile = new MediaFile(inputPath);

            var outputFile = new MediaFile(outputPath);

            var ffmpeg = new Engine(Path.Combine(Constants.FFmpegFolder, FFmpegPath));
            var options = new ConversionOptions();
            if (cutMediaTimeSpan > 0) options.CutMedia(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(cutMediaTimeSpan));

           await ffmpeg.ConvertAsync(inputFile, outputFile, options);
          
        }
        protected virtual string Run(string cmd)
        {
            cmd = cmd.Replace(@"""", @"\""");
            var argumets = $@"{Arguments} ""{cmd}""";
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = Constants.FFmpegFolder,
                    FileName = CmdName,
                    Arguments = argumets,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                process.Start();

                process.WaitForExit();
                process.Close();

                return argumets;
            }
        }
       
    }
}
