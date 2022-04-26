using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Infrastructure.Utilities
{
    /// <summary>
    /// 负责将对象序列化成byte[]数组
    /// </summary>
    public class SerHelper
    {

        /// <summary>
        /// 将实体序列化成byte[]数组
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] Serialize(object obj)
        {
            /* BinaryFormatter bf = new BinaryFormatter();
             byte[] res;
             using (MemoryStream ms = new MemoryStream())
             {
                 bf.Serialize(ms, obj);

                 res = ms.ToArray();
             }

             return res;*/
            int size = Marshal.SizeOf(obj);

            // Console.WriteLine(size);

            IntPtr buffer = Marshal.AllocHGlobal(size);

            try

            {

                Marshal.StructureToPtr(obj, buffer, false);

                Byte[] bytes = new Byte[size];

                Marshal.Copy(buffer, bytes, 0, size);

                return bytes;

            }

            finally

            {

                Marshal.FreeHGlobal(buffer);

            }
        }
        public static Object BytesToStruct(Byte[] bytes, Type strcutType)
        {

            Int32 size = Marshal.SizeOf(strcutType);

            IntPtr buffer = Marshal.AllocHGlobal(size);

            try

            {

                Marshal.Copy(bytes, 0, buffer, size);

                return Marshal.PtrToStructure(buffer, strcutType);

            }

            finally

            {

                Marshal.FreeHGlobal(buffer);

            }

        }

        /// <summary>
        /// 根据byte[]字节数组反序列化成 对象实体
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] buffer)
        {
            BinaryFormatter bf = new BinaryFormatter();
            T obj;
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                obj = (T)bf.Deserialize(ms);
            }
            return obj;
        }
    }
    public class StructHelper
    {
        //// <summary>
        /// 结构体转byte数组
        /// </summary>
        /// <param name="structObj">要转换的结构体</param>
        /// <returns>转换后的byte数组</returns>
        public static byte[] StructToBytes(Object structObj)
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(structObj);
            //创建byte数组
            byte[] bytes = new byte[size];
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将结构体拷到分配好的内存空间
            Marshal.StructureToPtr(structObj, structPtr, false);
            //从内存空间拷到byte数组
            Marshal.Copy(structPtr, bytes, 0, size);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回byte数组
            return bytes;
        }

        /// <summary>
        /// byte数组转结构体
        /// </summary>
        /// <param name="bytes">byte数组</param>
        /// <param name="type">结构体类型</param>
        /// <returns>转换后的结构体</returns>
        public static object BytesToStuct(byte[] bytes, Type type)
        {

            //object obj;
            //using (MemoryStream ms = new MemoryStream(bytes))
            //{
            //    IFormatter iFormatter = new BinaryFormatter();
            //    obj = iFormatter.Deserialize(ms);
            //}
            //return obj;
            //得到结构体的大小
            int size = Marshal.SizeOf(type);
            //byte数组长度小于结构体的大小
            if (size > bytes.Length)
            {
                //返回空
                return null;
            }
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            try
            {
                //将byte数组拷到分配好的内存空间
                Marshal.Copy(bytes, 0, structPtr, size);
                //将内存空间转换为目标结构体
                return Marshal.PtrToStructure(structPtr, type);
            }
            finally
            {
                //释放内存空间
                Marshal.FreeHGlobal(structPtr);
            }

        }

    }
}
