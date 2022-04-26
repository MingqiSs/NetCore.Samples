using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Infrastructure.Utilities
{
    public class UploadUtils
    {
        private static char[] base64CodeArray = new char[]
       {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4',  '5', '6', '7', '8', '9', '+', '/', '='
       };
        /// <summary>
        /// 是否base64字符串
        /// </summary>
        /// <param name="base64Str">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsBase64(string base64Str)
        {
            byte[] bytes = null;
            return IsBase64(base64Str, out bytes);
        }
        /// <summary>
        /// 是否base64字符串
        /// </summary>
        /// <param name="base64Str">要判断的字符串</param>
        /// <param name="bytes">字符串转换成的字节数组</param>
        /// <returns></returns>
        public static bool IsBase64(string base64Str, out byte[] bytes)
        {
            bytes = null;
            if (string.IsNullOrEmpty(base64Str))
                return false;
            else
            {
                if (base64Str.Contains(","))
                    base64Str = base64Str.Split(',')[1];
                if (base64Str.Length % 4 != 0)
                    return false;
                if (base64Str.Any(c => !base64CodeArray.Contains(c)))
                    return false;
            }
            try
            {
                bytes = Convert.FromBase64String(base64Str);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static byte[] GetBytesFromUrl(string url)
        {
            try
            {
                #region old
                //  HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
                //WebResponse myResp = myReq.GetResponse();

                //Stream stream = myResp.GetResponseStream();
                ////int i;
                //using (BinaryReader br = new BinaryReader(stream))
                //{
                //    // i = (int)(stream.Length);
                //     b = br.ReadBytes(500000);
                //    br.Close();
                //}
                //myResp.Close();
                #endregion
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) =>
                        {
                            return true;
                        };
                using (HttpClient client = new HttpClient(handler))
                {
                    return client.GetByteArrayAsync(url).Result;
                }
            }
            catch
            {
                return new byte[] { };
            }
        }

        public static bool HttpDownloadFile(string url, string path,string fileName)
        {
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
                WebResponse myResp = myReq.GetResponse();

                Stream responseStream = myResp.GetResponseStream();
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                
                //创建本地文件写入流
                Stream stream = new FileStream($"{path}/{fileName}", FileMode.Create);

                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    stream.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                stream.Close();
                responseStream.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 从网站上下载文件，保存到其他路径
        /// </summary>
        /// <param name="pdfFile">文件地址</param>
        /// <param name="saveLoadFile"></param>
        /// <returns></returns>
        public static string SaveRemoteFile(string url, string path, string fileName)
        {
            var f = $"{path}/{fileName}";
            #region old
            //Uri downUri = new Uri(url);
            ////建立一个ＷＥＢ请求，返回HttpWebRequest对象
            //HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(downUri);
            ////流对象使用完后自动关闭
            //using (Stream stream = hwr.GetResponse().GetResponseStream())
            //{
            //    //文件流，流信息读到文件流中，读完关闭
            //    using (Stream fs = new FileStream(f, FileMode.Create))
            //    {
            //        //建立字节组，并设置它的大小是多少字节
            //        byte[] bytes = new byte[102400];
            //        int n = 1;
            //        while (n > 0)
            //        {
            //            //一次从流中读多少字节，并把值赋给Ｎ，当读完后，Ｎ为０,并退出循环
            //            n = stream.Read(bytes, 0, 10240);
            //            fs.Write(bytes, 0, n);　//将指定字节的流信息写入文件流中
            //        }
            //    }
            //}
            #endregion
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
            using (HttpClient client = new HttpClient(handler))
            {
                using (Stream stream = client.GetStreamAsync(url).Result)
                {
                    //文件流，流信息读到文件流中，读完关闭
                    using (Stream fs = new FileStream(f, FileMode.Create))
                    {
                        //建立字节组，并设置它的大小是多少字节
                        byte[] bytes = new byte[102400];
                        int n = 1;
                        while (n > 0)
                        {
                            //一次从流中读多少字节，并把值赋给Ｎ，当读完后，Ｎ为０,并退出循环
                            n = stream.Read(bytes, 0, 10240);
                            fs.Write(bytes, 0, n); //将指定字节的流信息写入文件流中
                        }
                    }
                }
            }
            return f;
        }
        /// <summary>
        /// 图片地址转为Base64位字符串
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string NetImageToBase64(string url)
        {
            var b = GetBytesFromUrl(url);
            if (b.Length > 0)
            {
                return Convert.ToBase64String(b);
            }
            return string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Imagefilename"></param>
        public static byte[] GetBytesFromFilePath(string filePath)
        {
          
            byte[] pReadByte = new byte[0];
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    BinaryReader r = new BinaryReader(fs);
                    r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                    pReadByte = r.ReadBytes((int)r.BaseStream.Length);
                    return pReadByte;
                }
            }
            catch
            {
                return pReadByte;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public static byte[] GetBytesFormFile(IFormFile formFile)
        {
            try
            {
                using (var stram = formFile.OpenReadStream())
                {
                    byte[] buffer = new byte[stram.Length];
                    stram.Read(buffer, 0, buffer.Length);
                    stram.Seek(0, SeekOrigin.Begin);
                    return buffer;
                }
            }
            catch
            {
                return null;
            }
        }
        public static byte[] StreamToBytes(Stream stream)
        {
            if (stream == null)
            {
                return null;
            }

            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
        public static void WriteBytesToFile(string fileName, byte[] content)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs);
            try
            {
                w.Write(content);
            }
            finally
            {
                fs.Close();
                w.Close();
            }

        }

        /// <summary>
        /// 文件转换成Base64字符串
        /// </summary>
        /// <param name="fileName">文件绝对路径</param>
        /// <returns></returns>
        public static string FileToBase64(string fileName)
        {
            string strRet = null;

            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open);
                byte[] bt = new byte[fs.Length];
                fs.Read(bt, 0, bt.Length);
                strRet = Convert.ToBase64String(bt);
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strRet;
        }
        /// <summary>
        /// Base64字符串转换成文件
        /// </summary>
        /// <param name="strInput">base64字符串</param>
        /// <param name="fileName">保存文件的绝对路径</param>
        /// <returns></returns>
        public static bool Base64ToFileAndSave(string strInput, string fileName)
        {
            bool bTrue = false;

            try
            {
                byte[] buffer = Convert.FromBase64String(strInput);
                FileStream fs = new FileStream(fileName, FileMode.CreateNew);
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
                bTrue = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bTrue;
        }
        /// <summary>
        /// 将byte[]数组保存成文件
        /// </summary>
        /// <param name="byteArray">byte[]数组</param>
        /// <param name="fileName">保存至硬盘的文件路径</param>
        /// <returns></returns>
        public static bool ByteToFile(byte[] byteArray, string fileName)
        {
            bool result = false;
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 删除文件夹下的所有文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool DeleteDir(string file)
        {
            try
            {
                //去除文件夹和子文件的只读属性
                //去除文件夹的只读属性
                System.IO.DirectoryInfo fileInfo = new DirectoryInfo(file);
                fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

                //去除文件的只读属性
                System.IO.File.SetAttributes(file, System.IO.FileAttributes.Normal);

                //判断文件夹是否还存在
                if (Directory.Exists(file))
                {
                    foreach (string f in Directory.GetFileSystemEntries(file))
                    {

                        if (File.Exists(f))
                        {
                            //如果有子文件删除文件
                            File.Delete(f);
                        }
                        else
                        {
                            //循环递归删除子文件夹
                            DeleteDir(f);
                        }

                    }
                    Directory.Delete(file);
                }
                return true;
            }
            catch (Exception) // 异常处理
            {
                return false;
            }
        }

    }
}
