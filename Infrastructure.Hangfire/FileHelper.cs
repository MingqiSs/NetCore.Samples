using Hangfire.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Infrastructure.Hangfire
{
    public class FileHelper
    {
        public static List<T> ReadFile<T>(string fileName) where T : new()
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(fileName);
            }
            var configFile = File.Exists(fileName) ? fileName :
                Path.Combine(
#if NET45
				AppDomain.CurrentDomain.BaseDirectory,
#else
                AppContext.BaseDirectory,
#endif
                fileName);
            if (!File.Exists(configFile)) throw new FileNotFoundException($"The json file {configFile} does not exist.");
            try
            {
                var fileInfo = new FileInfo(configFile);
                var jsonContent = "";
                // Do stuff with file  
                using (var file = fileInfo.OpenRead())
                using (StreamReader reader = new StreamReader(file))
                    jsonContent = reader.ReadToEnd();

                var list = JobHelper.FromJson<List<T>>(jsonContent);
                return list;
            }
            catch (Exception ex) when (
            ex is IOException ||
            ex is UnauthorizedAccessException)
            {
                throw new Exception($"Read profile exceptions.({ex.Message})");
            }
        }
    }
}
