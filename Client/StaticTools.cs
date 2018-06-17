using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Forms;


namespace NanoChat
{
    public static class StaticTools
    {
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
        public static byte[] CombomBinaryArray(byte[] srcArray1, byte[] srcArray2, byte[] srcArray3,byte[] srcArray4)//连接4个字节数组
        {
            byte[] newArray = new byte[srcArray1.Length + srcArray2.Length + srcArray3.Length+srcArray4.Length];
            Array.Copy(srcArray1, 0, newArray, 0, srcArray1.Length);
            Array.Copy(srcArray2, 0, newArray, srcArray1.Length, srcArray2.Length);
            Array.Copy(srcArray3, 0, newArray, srcArray1.Length + srcArray2.Length, srcArray3.Length);
            Array.Copy(srcArray4, 0, newArray, srcArray1.Length + srcArray2.Length + srcArray3.Length, srcArray4.Length);
            return newArray;
        }
        public static Byte[] BmpConvertByte(Image image)//图片转字节流
        {
            MemoryStream ms1 = new MemoryStream();
            image.Save(ms1, System.Drawing.Imaging.ImageFormat.Bmp);
            return ms1.GetBuffer();
        }
        public static System.Drawing.Image ReturnPhoto(byte[] streamByte)//字节流转图片
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(streamByte);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            return img;
        }
        public static byte[] GetPictureBytes(string filename)       //filename填写图片路径  
        {
            FileInfo fileInfo = new FileInfo(filename);
            byte[] buffer = new byte[fileInfo.Length];
            using (FileStream stream = fileInfo.OpenRead())
            {
                stream.Read(buffer, 0, buffer.Length);
            }
            return buffer;
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
        public static void InsertImage(RichTextBox rtb1 ,Image img)
        {
            bool b = rtb1.ReadOnly;
            //Image img = Image.FromFile("sss.bmp");
            Clipboard.SetDataObject(img);
            rtb1.ReadOnly = false;
            rtb1.Paste(DataFormats.GetFormat(DataFormats.Bitmap));
            rtb1.ReadOnly = b;
        }
        public static bool IsChineseSimple()        //当前操作系统是否为简体中文
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.Name == "zh-CN";
        }
        public static class OSHelper//系统助手
        {
            public static bool IsUnix
            {
                get
                {
                    return Environment.OSVersion.Platform == PlatformID.Unix;//返回当前系统是否是unix系统
                }
            }
        }
    }
}
