using Infrastructure.Const;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Infrastructure.Utilities.FFmpeg
{
    public class LinuxFFmpeg : FFmpegBase
    {
        /// <summary>
        /// Linux下需要绝对路径，否则会识别为全局命令（除非系统本身已安装FFmpeg）
        /// </summary>
        public override string FFmpegPath => Path.Combine(Constants.FFmpegFolder, "ffmpeg");

        public override string CmdName => "/bin/bash";

        public override string Arguments => "-c";
    }
}
