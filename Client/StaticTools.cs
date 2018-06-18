using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Net.Sockets;

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
        public static byte[] GetFileBytes(string filename)       //filename填写图片路径  
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
            try
            {
                Clipboard.SetDataObject(img);
            }
            catch (Exception ee) {
                MessageBox.Show(ee.Message);
            }
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
        public static string AppendTimeStamp(string fileName)//在文件尾部加入时间戳
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(fileName),
                DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Path.GetExtension(fileName)
                );
        }
        public static string ReceivePic(int rect,string sendside,byte[] data,Socket tsocket){
                FileStream wrtr; //文件读写类  
                //server.Listen(10); //监听  
                //Socket s = server.Accept(); //当有客户端与服务器进行连接，Accept方法返回socket对象，通过该对象可以获取客户端发送的消息  
                //byte[] data = new byte[4];  
                //int rect = tsocket.Receive(data, 2, 4, 0); //用来接收图片字节流长度  
                int size = BitConverter.ToInt32(data, 2);  //16进制转成int型  
                int dataleft = size;   
                data = new byte[size];  //创建byte组
                string filepath = @"./savepicture/new in.png";
                string foldpath = Path.GetDirectoryName(filepath);
                Directory.CreateDirectory(foldpath);//如果没有文件夹，则创建
                //filepath = StaticTools.AppendTimeStamp(filepath);
                filepath = Path.Combine(foldpath, StaticTools.AppendTimeStamp(filepath));//加入时间戳，并与前置路径连接
                wrtr = new FileStream(filepath , FileMode.Create);
                //创建新文件"new.jpg",strPath是路径   
                //data = new byte[2048];
                int total = 0;  
                while (total < size)   //当接收长度小于总长度时继续执行  
                {  
                rect = tsocket.Receive(data, total, dataleft, 0);    //接收字节流，receive方法返回int获取已接收字节个数，第一个参数是需要写入的字节组，第二个参数是起始位置，第三个参数是接收字节的长度  
                total += rect;            //已接收个数-下一次从当前个数开始接收  
                dataleft -= rect;  //剩下的字节长度  
                }  
                wrtr.Write(data, 0, data.Length); //输出文件  
                wrtr.Flush();  //强制输出
                wrtr.Close();  //关闭文件流对象
                return filepath;
                }
        //public static string defaultpicpath = @"./savepicture";
        //public static string defaultfilepath = @"./savefile";
        public static string ReceiveFile(int rect, string sendside, string fileext, byte[] data, Socket tsocket)
        {
            FileStream wrtr; //文件读写类  
            //server.Listen(10); //监听  
            //Socket s = server.Accept(); //当有客户端与服务器进行连接，Accept方法返回socket对象，通过该对象可以获取客户端发送的消息  
            //byte[] data = new byte[4];  
            //int rect = tsocket.Receive(data, 2, 4, 0); //用来接收图片字节流长度  
            int size = BitConverter.ToInt32(data, 2);  //16进制转成int型  
            int dataleft = size;
            data = new byte[size];  //创建byte组
            string filepath = @"./savefile/newfile in ";
            string foldpath = Path.GetDirectoryName(filepath);
            Directory.CreateDirectory(foldpath);//如果没有文件夹，则创建
            //filepath = StaticTools.AppendTimeStamp(filepath);
            filepath = Path.Combine(foldpath, StaticTools.AppendTimeStamp(filepath));//加入时间戳，并与前置路径连接
            filepath = Path.ChangeExtension(filepath, fileext);
            wrtr = new FileStream(filepath, FileMode.Create);
            //创建新文件"new.jpg",strPath是路径   
            //data = new byte[2048];
            int total = 0;
            while (total < size)   //当接收长度小于总长度时继续执行  
            {
                rect = tsocket.Receive(data, total, dataleft, 0);    //接收字节流，receive方法返回int获取已接收字节个数，第一个参数是需要写入的字节组，第二个参数是起始位置，第三个参数是接收字节的长度  
                total += rect;            //已接收个数-下一次从当前个数开始接收  
                dataleft -= rect;  //剩下的字节长度  
            }
            wrtr.Write(data, 0, data.Length); //输出文件  
            wrtr.Flush();  //强制输出
            wrtr.Close();  //关闭文件流对象
            //File.Delete(strpath);
            return filepath;
        }
    }
}
