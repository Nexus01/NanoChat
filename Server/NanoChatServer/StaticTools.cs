using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NanoChatServer
{
    class StaticTools
    {
        public class OSHelper
        {
            public static bool IsUnix
            {
                get
                {
                    return Environment.OSVersion.Platform == PlatformID.Unix;////返回当前系统是否是unix系统
                }
            }
        }
        public static bool ConvertIntToByteArray(Int32 m, ref byte[] arry)
        {
            if (arry == null)
                return false;
            if (arry.Length < 4)
                return false;
            arry[0] = (byte)(m & 0xFF);
            arry[1] = (byte)((m & 0xFF00) >> 8);
            arry[2] = (byte)((m & 0xFF0000) >> 16);
            arry[3] = (byte)((m >> 24) & 0xFF);
            return true;
        }
        public static byte[] CombomBinaryArray(byte[] srcArray1, byte[] srcArray2)//连接2个字节数组
        {
            byte[] newArray = new byte[srcArray1.Length + srcArray2.Length];
            Array.Copy(srcArray1, 0, newArray, 0, srcArray1.Length);
            Array.Copy(srcArray2, 0, newArray, srcArray1.Length, srcArray2.Length);
            return newArray;
        }
        public static byte[] CombomBinaryArray(byte[] srcArray1, byte[] srcArray2, byte[] srcArray3)//连接3个字节数组
        {
            byte[] newArray = new byte[srcArray1.Length + srcArray2.Length + srcArray3.Length];
            Array.Copy(srcArray1, 0, newArray, 0, srcArray1.Length);
            Array.Copy(srcArray2, 0, newArray, srcArray1.Length, srcArray2.Length);
            Array.Copy(srcArray3, 0, newArray, srcArray1.Length + srcArray2.Length, srcArray3.Length);
            return newArray;
        }
        public static byte[] CombomBinaryArray(byte[] srcArray1, byte[] srcArray2, byte[] srcArray3, byte[] srcArray4)//连接4个字节数组
        {
            byte[] newArray = new byte[srcArray1.Length + srcArray2.Length + srcArray3.Length + srcArray4.Length];
            Array.Copy(srcArray1, 0, newArray, 0, srcArray1.Length);
            Array.Copy(srcArray2, 0, newArray, srcArray1.Length, srcArray2.Length);
            Array.Copy(srcArray3, 0, newArray, srcArray1.Length + srcArray2.Length, srcArray3.Length);
            Array.Copy(srcArray4, 0, newArray, srcArray1.Length + srcArray2.Length + srcArray3.Length, srcArray4.Length);
            return newArray;
        }
        public static byte[] CombomBinaryArray(byte[] srcArray1, byte[] srcArray2, byte[] srcArray3, byte[] srcArray4, byte[] srcArray5)//连接5个字节数组
        {
            byte[] newArray = new byte[srcArray1.Length + srcArray2.Length + srcArray3.Length + srcArray4.Length + srcArray5.Length];
            Array.Copy(srcArray1, 0, newArray, 0, srcArray1.Length);
            Array.Copy(srcArray2, 0, newArray, srcArray1.Length, srcArray2.Length);
            Array.Copy(srcArray3, 0, newArray, srcArray1.Length + srcArray2.Length, srcArray3.Length);
            Array.Copy(srcArray4, 0, newArray, srcArray1.Length + srcArray2.Length + srcArray3.Length, srcArray4.Length);
            Array.Copy(srcArray5, 0, newArray, srcArray1.Length + srcArray2.Length + srcArray3.Length + srcArray4.Length, srcArray5.Length);
            return newArray;
        }
        public static string AppendTimeStamp(string fileName)//在文件尾部加入时间戳
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(fileName),
                DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Path.GetExtension(fileName)
                );
        }
    }
}
