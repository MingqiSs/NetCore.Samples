using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Infrastructure.Utilities
{
        public class BigEndianUIntHelper
        {
      
        public static int ToInt32(byte[] data)
        {
            Array.Reverse(data);
            return BitConverter.ToInt32(data);
        }
        public static int ToInt32ByPrice(byte[] data)
        {
            Array.Reverse(data);
            //return BitConverter.ToInt32(data) /10;
            return (int)Math.Round((double)BitConverter.ToInt32(data) / 10); 
        }
        public static long ToInt64(byte[] data)
        {
            Array.Reverse(data);
            return BitConverter.ToInt64(data);
        }
        /// <summary>
        /// Long Price
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int ToInt64ByPrice(byte[] data)
        {
            Array.Reverse(data);
            //return BitConverter.ToInt32(data) /10;
            return (int)Math.Round((double)BitConverter.ToInt64(data) / 10);
        }
        public static byte[] ToBytes(uint num, int size)
            {
                Contract.Assume(size <= 4);
                return BitConverter.GetBytes(num).Take(size).Reverse().ToArray();
            }
        }
}
