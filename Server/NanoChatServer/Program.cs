using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace NanoChatServer
{
    public class Server
    {
        int port;
        static List<Socket> clients;
        TcpListener server = null;//创建tcp监听器
        public Server()
        {
            try
            {
                port = 8899; //端口号
                clients = new List<Socket> (); //实例化socket list对象
                server = new TcpListener(IPAddress.Any, port);//设置监听ip和port
                while (true)
                {
                    server.Start();//开启服务器
                    Socket socket = server.AcceptSocket(); //死循环监听客户端连接
                    clients.Add(socket); //向保存客户端socket list添加socket实例
                    Mythread methread = new Mythread(socket, clients.Count - 1);//实例化Mythread对象，并调用构造函数传送当前socket实例id
                    Thread mythread = new Thread(new ThreadStart(methread.run)); //实例化新线程并采用委托方式
                    mythread.Start(); //开启一个新线程
                }
            }
            catch (Exception)
            {
            }
        }
        
        public class Mythread
        { //子线程负责每个客户端消息推送
            Socket tsocket;        
            public string msg;
            public int id;
            public Mythread(Socket s, int @is)
            {                
                tsocket = s;
                id = id;
            }
            byte[] myReadBuffer = new byte[1024];//创建缓冲区
            int numberofinput = 0;
            static string originmsg;
            public void run()
            {
                try
                {
                    NetworkStream networkStream = new NetworkStream(tsocket);
                    bool GBK = false;
                    //msg = "[IP:" + IPAddress.Parse(((IPEndPoint)tsocket.LocalEndPoint).Address.ToString()) + "已上线] 在线人数:" + clients.Count;
                    msg = "[IP:" + IPAddress.Parse(((IPEndPoint)tsocket.LocalEndPoint).Address.ToString()) + " online] the number of online is:" + clients.Count;
                    myReadBuffer = Encoding.UTF8.GetBytes(msg);//以UTF8编码方式读取msg中字节
                    sendMsg(msg.Length,GBK); //上线推送
                    tsocket.Receive(myReadBuffer);//接受socket
                    byte[] gb;
                    if (!(Convert.ToBoolean(myReadBuffer[0])))//将接受到的1（unix)或者0（！unix)转换为bool类型，判断是否需要转码
                    { 
                        GBK = true;//需要从UTF8转码成GBK
                    }
                        while (true)
                        //while (!string.ReferenceEquals((msg = Encoding.UTF8.GetString(myReadBuffer)), null))
                        {
                            if ((numberofinput = networkStream.Read(myReadBuffer, 0, myReadBuffer.Length)) < 0)
                                break;
                            msg = Encoding.UTF8.GetString(myReadBuffer, 0, numberofinput);//将缓冲区中客户端输入的转为string
                            //UTF8 to GBK
                            if (GBK)
                            {
                                originmsg = msg;//储存原始msg
                                msg = Encoding.GetEncoding("GBK").GetString(Encoding.UTF8.GetBytes(msg));//转码
                            }
                            if (msg.Equals("exitthis"))
                            { //如果收到exitthis则退出线程，回收socket。
                                clients.RemoveAt(id);
                                break;
                            }
                            sendMsg(numberofinput,GBK); //推送消息到所有客户端
                        }
                    
                }
                catch (Exception)
                {
                }
            }
            //发送信息
            public void sendMsg(int number,bool GBK)
            {
                try
                {
                    if (GBK)
                    {   //将msg由UTF8转为GBK
                        msg = Encoding.GetEncoding("GBK").GetString(Encoding.Convert(Encoding.GetEncoding("GBK"), Encoding.UTF8, Encoding.GetEncoding("GBK").GetBytes(msg)));
                        Console.WriteLine(originmsg);//在服务端打印客户端输入
                    }
                    else
                        Console.WriteLine(msg);//在服务端打印客户端输入
                    for (int i = clients.Count - 1; i >= 0; i--)
                    { //循环推送
                        NetworkStream pw = new NetworkStream(clients[i]);//为i号客户端创建新NetworkStream
                        pw.Write(myReadBuffer,0,number);//将客户端发信写入缓存
                        pw.Flush();//将缓存中的数据发送出去
                    }
                }
                catch (Exception)
                {
                }
            }
        }
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
        public static void Main(string[] args)
        {
            new Server();
        }
    }
}



