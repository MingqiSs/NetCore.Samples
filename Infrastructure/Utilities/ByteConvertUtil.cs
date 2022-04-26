using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class ByteConvertUtil
    {

        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        /// <summary>
        /// 更新字节数组中的某一段，不允许越界
        /// </summary>
        /// <param name="bytes0"></param>
        /// <param name="offset"></param>
        /// <param name="bytes1"></param>
        /// <returns></returns>
        public static byte[] BufferUpdate(byte[] bytes0, int offset, byte[] bytes1)
        {
            byte[] resultBytes = new byte[bytes0.Length];
            Buffer.BlockCopy(bytes0, 0, resultBytes, 0, offset);
            Buffer.BlockCopy(bytes1, 0, resultBytes, offset, bytes1.Length);
            return resultBytes;
        }
        public static byte[] BufferCopy(byte[] bytes0, byte[] bytes1)
        {
            byte[] resultBytes = new byte[bytes0.Length + bytes1.Length];

            Buffer.BlockCopy(bytes0, 0, resultBytes, 0, bytes0.Length);
            Buffer.BlockCopy(bytes1, 0, resultBytes, bytes0.Length, bytes1.Length);

            return resultBytes;
        }
        public static byte[] ArrayCopy(byte[] bytes0, byte[] bytes1)
        {
            byte[] resultBytes = new byte[bytes0.Length + bytes1.Length];
            Array.Copy(bytes0, 0, resultBytes, 0, bytes0.Length);
            Array.Copy(bytes1, 0, resultBytes, bytes0.Length, bytes1.Length);
            return resultBytes;
        }
    }
}
