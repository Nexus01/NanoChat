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
                port = 2018; //端口号
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
            Socket tsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);        
            public string msg;
            public int id;
            public Mythread(Socket s, int ID)
            {                
                tsocket = s;
                id = ID;
            }
            byte[] myReadBuffer = new byte[1024];//创建缓冲区
            byte[] yourReadBuffer = new byte[32];
            int numberofinput = 0;
            int namecount = 1;
            static string originmsg;
            string username;
            
            int colonindex=100;
            public void run()
            {
                try
                {
                    bool GBK = false;
                    //byte[] tempname;
                    
                    //
                    NetworkStream networkStream = new NetworkStream(tsocket);
                    int receive = tsocket.Receive(myReadBuffer);//接受系统和用户名socket
                    if (!(Convert.ToBoolean(myReadBuffer[0])))//将接受到的1（unix)或者0（！unix)转换为bool类型，判断是否需要转码
                    {
                        GBK = true;//需要从UTF8转码成GBK
                        //Console.WriteLine("GBK"+GBK.ToString());
                    }
                    username = Encoding.UTF8.GetString(myReadBuffer, 1, receive-1);
                    //Console.WriteLine(username);
                    //msg = "[IP:" + IPAddress.Parse(((IPEndPoint)tsocket.LocalEndPoint).Address.ToString()) + "已上线] 在线人数:" + clients.Count + "\n";
                    msg = "[IP:" + IPAddress.Parse(((IPEndPoint)tsocket.LocalEndPoint).Address.ToString()) + " online] the number of online is:" + clients.Count+"\n";
                    myReadBuffer = Encoding.UTF8.GetBytes(msg);//以UTF8编码方式读取msg中字节
                    
                    sendMsg(msg.Length,GBK); //上线推送
                    myReadBuffer = new byte[2048];
                    //byte[] gb;
                    
                    
                    //Array.Clear(myReadBuffer, 0, myReadBuffer.Length);
                    //tsocket.Receive(myReadBuffer);//接收用户名的socket

                    
                        while (true)
                        //while (!string.ReferenceEquals((msg = Encoding.UTF8.GetString(myReadBuffer)), null))
                        {
                            //if (clients.Count > 0)
                            //{
                            //    for (int i = clients.Count - 1; i >= 0; i--)
                            //    {
                            //        if (clients[i].Poll(1000, SelectMode.SelectRead)) //SelectMode.SelectRead表示，如果已调用 并且有挂起的连接，true。
                            //        {
                            //            clients[i].Close();//关闭socket
                            //            clients.RemoveAt(id);//从列表中删除断开的socke
                            //            continue;
                            //        }
                            //    }
                            //}
                            if ((numberofinput = networkStream.Read(myReadBuffer, 0, myReadBuffer.Length)) <= 0)
                                break;
                            msg = Encoding.UTF8.GetString(myReadBuffer, 0, numberofinput);//将缓冲区中客户端输入的转为string
                            //UTF8 to GBK
                            
                            if (GBK)
                            {
                                originmsg = msg;//储存原始msg
                                //Console.WriteLine(msg);
                            }
                            if (msg.Equals("exitthis"))
                            { //如果收到exitthis则退出线程，回收socket。
                                Console.WriteLine("A user will leave the chatroom whose login id is "+id+" and username is "+ username);
                                clients.RemoveAt(id);
                                //msg = Encoding.GetEncoding("GBK").GetString(Encoding.UTF8.GetBytes(username));
                                msg = "A user will leave the chatroom whose login id is " + id + " and username is " + username+ "\n";
                                myReadBuffer = Encoding.UTF8.GetBytes(msg);
                                sendMsg(msg.Length,GBK);//推送用户下线消息
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
            public void sendPic(){
            FileStream wrtr; //文件读写类  
            //server.Listen(10); //监听  
            //Socket s = server.Accept(); //当有客户端与服务器进行连接，Accept方法返回socket对象，通过该对象可以获取客户端发送的消息  
            byte[] data = new byte[4];  
            int rect = tsocket.Receive(data, 0, 4, 0); //用来接收图片字节流长度  
            int size = BitConverter.ToInt32(data, 0);  //16进制转成int型  
            int dataleft = size;   
            data = new byte[size];  //创建byte组  
            string strpath=@".\new.png";
            wrtr = new FileStream(strpath , FileMode.Create);
                //创建新文件"new.jpg",strPath是路径  
  
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



