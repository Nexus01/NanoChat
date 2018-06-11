using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace NanoChat
{
    public partial class Form1 : Form
    {
        //private delegate void UpdateStatusDelegate(string status);
        //public delegate void MyInvoke();
        Thread thread;//子线程对象
        
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        /*private void DoWork()
        {
            //委托对象
            MyInvoke mi = new MyInvoke(netscan);
            //异步调用委托
            this.BeginInvoke(mi);
        }*/
        private void Form1_Load(object sender, EventArgs e)//窗口初始化函数
        {
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);//注册窗口X函数事件
            this.thread = new Thread(new ThreadStart(this.netscan));//实例化子线程
            this.thread.Start();//开启子线程
            this.ipaddr.Text = Netconfig.crossip;
            this.port.Text = Netconfig.crossport;
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {}
        private void netscan(){
            string ip1 = "114.114.114.114";
            //string ip2 = "8.8.8.8";
            while (true)
            {
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();

                // Use the default Ttl value which is 128,
                // but change the fragmentation behavior.
                options.DontFragment = true;

                // Create a buffer of 32 bytes of data to be transmitted.
                //string data = "judge online or not";
                //byte[] buffer = Encoding.UTF8.GetBytes(data);
                int timeout = 120;
                int counteroftimeout = 0;
                int counterofsuccess = 0;
                int counterofdisconnect = 0;
                int[] delayms = new int[13];
                string[] situation = { "优", "良", "差" };
                string netresult;
                for (int j = 0; j < 13; j++)
                    delayms[j] = 0;
                for (int counter = 0; counter < 13; counter++)
                {
                    try
                    {
                        PingReply reply = pingSender.Send(ip1, timeout); //options);

                        networktest.Text = "检测中";
                        if (reply.Status == IPStatus.Success)
                        {
                            counterofsuccess++;
                            delayms[counter] = int.Parse((reply.RoundtripTime).ToString());
                        }
                        else if (reply.Status == IPStatus.DestinationUnreachable)
                            networktest.Text = "无法访问目标";
                        //Console.WriteLine("无法访问目标");
                        else if (reply.Status == IPStatus.TimedOut)
                        {
                            counteroftimeout++;
                            //Console.WriteLine("请求超时");
                        }
                    }
                    catch (PingException)
                    {
                        counterofdisconnect++;
                        //MessageBox.Show("PING:传输失败。请检查联网状态");
                        //Console.WriteLine("PING:传输失败。General failure");
                        //Console.WriteLine("请检查联网状态");
                    }

                }
                Thread.Sleep(2000);
                if (counterofsuccess > counteroftimeout)
                {
                    //Console.WriteLine("connect success");
                    netresult = "已联网";
                    
                    networktest.Text = netresult;
                    Thread.Sleep(1000);
                    int averagedelay = 0;
                    string nowsitution = "优";
                    for (int m = 0; m < 13; m++)
                    {
                        averagedelay += delayms[m];
                    }
                    averagedelay = averagedelay / 13;
                    networktest.ForeColor = Color.Green;
                    if (averagedelay > 40 && averagedelay < 70)
                    {
                        nowsitution = situation[1];
                        networktest.ForeColor = Color.Yellow;
                    }
                    else if (averagedelay >= 70)
                    {
                        nowsitution = situation[2];
                        networktest.ForeColor = Color.Red;
                    }
                    netresult = string.Format(averagedelay.ToString() + "ms ");
                    //BeginInvoke(new MethodInvoker(delegate()
                    //{
                    //BeginInvoke
                    networktest.Text = netresult;//在新线程中更新UI控件
                    //Thread.Sleep(2000);
                    networktest.AppendText(nowsitution);
                    Thread.Sleep(2000);
                    networktest.Clear();
                    //}));

                    //Console.WriteLine("the average delay is " + averagedelay.ToString());
                    //Console.WriteLine("the situation of the Network is  " + nowsitution);
                }
                else if (counterofsuccess < counteroftimeout)
                    MessageBox.Show("请求超时，请确保网络畅通");
                else if (counterofdisconnect > 0)
                    MessageBox.Show("请检查联网设置");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ipaddr.Text.Equals("") && !name.Text.Equals("") && !port.Text.Equals("") && int.Parse(port.Text) > 1024 && int.Parse(port.Text) < 65535)//判断ip地址和用户名是否为空
                {
                    Form2 f2 = new Form2(this, ipaddr.Text, name.Text, port.Text);//实例化一个新窗口
                    f2.Show();//打开新窗口
                    this.Hide();//隐藏当前窗口
                }
                else if (int.Parse(port.Text) <= 1024 || int.Parse(port.Text) > 65535)
                    MessageBox.Show("输入的端口号有误(小于等于1024或者大于65535)");
                else
                {
                    MessageBox.Show("请输入您的IP或用户名还有端口！", "聊天室", MessageBoxButtons.OK, MessageBoxIcon.Question);//如果其中一个为空，则弹出
                }
            }
            catch (FormatException) 
            {
                MessageBox.Show("输入格式有误！");
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)//点击窗口x调用该函数
        {
            if (DialogResult.No == MessageBox.Show("您确定要退出登录吗?", "聊天室", MessageBoxButtons.YesNo, MessageBoxIcon.Question))//弹出提示
            {
                e.Cancel = true;
            }
            if (thread != null)
            {
                thread.Abort();
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }  
        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void settings_Click(object sender, EventArgs e)
        {
            settings f3 = new settings(this);
            f3.Show();
            //f3.TopMost = true;
            this.Hide();
        }

        
 
    }
    public static class Netconfig
    {
        public static string crossip = "127.1";
        public static string crossport = "2018";
    }
}
