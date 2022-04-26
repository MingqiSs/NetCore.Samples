using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.DEncrypt
{
    public class SHA256Encrypt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string SHA256Content(string content)
        {
            char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            using(var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(content);
                byte[] md = sha256.ComputeHash(bytes);
                int j = md.Length;
                char[] str = new char[j * 2];
                int k = 0;
                for(int i = 0; i < j; i++)
                {
                    byte byte0 = md[i];
                    str[k++] = hexDigits[byte0 >> 4 & 0xf];
                    str[k++] = hexDigits[byte0 & 0xf];
                }
                return new string(str);
            }
        }
        public static string Encrypt(string strIN)
        {
            byte[] tmpByte;
            SHA256 sha256 = new SHA256Managed();
            tmpByte = sha256.ComputeHash(GetKeyByteArray(strIN));

            StringBuilder rst = new StringBuilder();
            for (int i = 0; i < tmpByte.Length; i++)
            {
                rst.Append(tmpByte[i].ToString("x2"));
            }
            sha256.Clear();
            return rst.ToString();
        }
        private static byte[] GetKeyByteArray(string strKey)
        {
            UTF8Encoding Asc = new UTF8Encoding();
            int tmpStrLen = strKey.Length;
            byte[] tmpByte = new byte[tmpStrLen - 1];
            tmpByte = Asc.GetBytes(strKey);
            return tmpByte;
        }
        public static string HmacSHA256(string join_str, string secret)
        {
            StringBuilder sb = new StringBuilder();

            var encoding = new ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(secret);

            byte[] messageBytes = encoding.GetBytes(join_str);

            using (var hmacsha256 = new HMACSHA256(keyByte))

            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                foreach (byte b in hashmessage)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
