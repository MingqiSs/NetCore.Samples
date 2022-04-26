using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utilities.FFmpeg
{
    public interface IFFmpeg
    {
        /// <summary>
        /// 格式转换
        /// </summary>
        /// <param name="inputPath">输入文件路径</param>
        /// <param name="outputPath">输出文件路径</param>
        /// <returns></returns>
        string Convert(string inputPath, string outputPath);
        /// <summary>
        /// 格式转换
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        /// <param name="cutMediaTimeSpan"></param>
        /// <returns></returns>
        Task ConvertByFFmpegNetAsync(string inputPath, string outputPath, int cutMediaTimeSpan);
    }
}
