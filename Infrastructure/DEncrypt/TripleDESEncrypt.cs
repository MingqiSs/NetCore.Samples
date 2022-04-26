using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.DEncrypt
{
    public class TripleDESEncrypt
    {
        public static string Encrypt(string input, string key)
        {

            byte[] inputArray = Encoding.UTF8.GetBytes(input);
            var tripleDES = TripleDES.Create();
            var byteKey = Encoding.UTF8.GetBytes(key);
            byte[] allKey = new byte[24];
            Buffer.BlockCopy(byteKey, 0, allKey, 0, 16);
            Buffer.BlockCopy(byteKey, 0, allKey, 16, 8);
            tripleDES.Key = allKey;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string input, string key)
        {
            byte[] inputArray = Convert.FromBase64String(input);
            var tripleDES = TripleDES.Create();
            var byteKey = Encoding.UTF8.GetBytes(key);
            byte[] allKey = new byte[24];
            Buffer.BlockCopy(byteKey, 0, allKey, 0, 16);
            Buffer.BlockCopy(byteKey, 0, allKey, 16, 8);
            tripleDES.Key = allKey;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }
        public static byte[] Encrypt(string input, byte[] key)
        {
            byte[] inputArray = Encoding.Default.GetBytes(input);
            var tripleDES = TripleDES.Create();
            // var byteKey = Encoding.UTF8.GetBytes(key);
            byte[] allKey = new byte[24];

            if (key.Length == 16)
            {
                Buffer.BlockCopy(key, 0, allKey, 0, key.Length);
                Buffer.BlockCopy(key, 0, allKey, 16, 8);
            }
            else
            {
                Buffer.BlockCopy(key, 0, allKey, 0, 24);
            }
            tripleDES.Key = allKey;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.Zeros;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            return cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            //   return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static byte[] Decrypt(byte[] input, byte[] key)
        {
            var Base64Str = Convert.ToBase64String(input);
            byte[] inputArray = Convert.FromBase64String(Base64Str);
            var tripleDES = TripleDES.Create();
            byte[] allKey = new byte[24];
            if (key.Length == 16)
            {
                Buffer.BlockCopy(key, 0, allKey, 0, key.Length);
                Buffer.BlockCopy(key, 0, allKey, 16, 8);
            }
            else
            {
                Buffer.BlockCopy(key, 0, allKey, 0, 24);
            }
            tripleDES.Key = allKey;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.Zeros;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] dec = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            return dec;
        }
    }
}
