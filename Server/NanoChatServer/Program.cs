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
        TcpListener server = null;
        public Server()
        {
            try
            {
                port = 8899; //端口号
                clients = new List<Socket> (); //实例化socket list对象
                server = new TcpListener(IPAddress.Any, port);//开启服务器socket
                while (true)
                {
                    server.Start();
                    Socket socket = server.AcceptSocket(); //死循环监听客户端连接
                    clients.Add(socket); //向保存客户端socket list添加socket实例
                    Mythread methread=new Mythread(socket, clients.Count- 1);
                    Thread mythread = new Thread(new ThreadStart(methread.run));//socket, clients.Count - 1)); //实例化新线程并传送当前socket实例id
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
            byte[] myReadBuffer = new byte[1024];
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
                    myReadBuffer = Encoding.UTF8.GetBytes(msg);
                    sendMsg(msg.Length,GBK); //上线推送
                    tsocket.Receive(myReadBuffer);
                    //byte[] unix = BitConverter.GetBytes(OSHelper.IsUnix);
                    byte[] gb;
                    //bool clientunix=false;
                    if (!(Convert.ToBoolean(myReadBuffer[0])))
                    { //^Convert.ToBoolean(unix[0]))
                        GBK = true;
                    }
                        while (true)
                        //while (!string.ReferenceEquals((msg = Encoding.UTF8.GetString(myReadBuffer)), null))
                        {
                            //int result = ;
                            if ((numberofinput = networkStream.Read(myReadBuffer, 0, myReadBuffer.Length)) < 0)
                                break;

                            msg = Encoding.UTF8.GetString(myReadBuffer, 0, numberofinput);
         
                            if (GBK)
                            {
                                originmsg = msg;
                                //Encoding gbk=Encoding.GetEncoding("GBK");
                                //Encoding utf8 = Encoding.UTF8;
                                msg = Encoding.GetEncoding("GBK").GetString(Encoding.UTF8.GetBytes(msg));
                                //gb= Encoding.GetEncoding("GBK").GetBytes(msg);
                                //gb = Encoding.Convert(Encoding.GetEncoding("GBK"), Encoding.UTF8, gb);
                            }
                            if (msg.Equals("exitthis"))
                            { //如果收到exitthis则退出线程，回收socket。
                                clients.RemoveAt(id);
                                break;
                            }
                            //msg=Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(msg));
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
                    {
                        //string originmsg = Encoding.UTF8.GetString(Encoding.Convert(Encoding.GetEncoding("GBK"), Encoding.UTF8, Encoding.GetEncoding("GBK").GetBytes(msg)));
                        msg = Encoding.GetEncoding("GBK").GetString(Encoding.Convert(Encoding.GetEncoding("GBK"), Encoding.UTF8, Encoding.GetEncoding("GBK").GetBytes(msg)));
                        Console.WriteLine(originmsg);
                    }
                    else
                        Console.WriteLine(msg);
                    for (int i = clients.Count - 1; i >= 0; i--)
                    { //循环推送
                        NetworkStream pw = new NetworkStream(clients[i]);
                        pw.Write(myReadBuffer,0,number);//(msg,0,msg.Length);
                        pw.Flush();
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
                    return Environment.OSVersion.Platform == PlatformID.Unix;
                }
            }
        }
        public static void Main(string[] args)
        {
            new Server();
        }
    }
}



