using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Resources;

namespace NanoChat
{
    public partial class Form2 : Form
    {
        Form f1;//用于保存form1传过来的对象
        String ip, name,port;//传过来的ip，name和port
        IPAddress ipdomain;//传过来的domain
        Thread thread;//子线程对象
        Socket newtcpclient;//Socket网络对象
        int flag ;//网络标志
        //bool unix = false;
        static List<byte[]> pictobyte=new List<byte[]>();
        static int[] pictidx=new int[10];
        public Form2(Form f1,String ip,String name,String port)
        {
            this.f1 = f1;//接收登录窗口form1对象
            this.ip = ip;//接收ip字符
            this.name = name;//接收用户名字符
            this.port = port;//接收port字符
            InitializeComponent();
            InitImageControl(12, 6);//开启内置表情
            this.richTextBox2.TabIndex = 0;
            Control.CheckForIllegalCrossThreadCalls = false;//关闭子线程刷新ui限制
            
           
        }

        private void Form2_Load(object sender, EventArgs e)//窗口初始化函数
        {
            flag = 0;//默认网络标志位0，表示接通网络
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);//注册窗口X事件
            ToolTip toolTip1 = new ToolTip();

            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 100;
            toolTip1.ReshowDelay = 200;
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(this.button2, "表情");
            toolTip1.SetToolTip(this.button3, "截屏");
            toolTip1.SetToolTip(this.button4, "文件");
            toolTip1.SetToolTip(this.button5, "图片");
            
            //this.richrichTextBox2.View = View.List;//设置Viewlist显示模式为列表模式
            //this.richrichTextBox2.Text=
            this.thread = new Thread(new ThreadStart(this.recv));//实例化子线程
            //this.thread.IsBackground = true;
            this.thread.Start();//开启子线程
            
            
        }
        private void recv()//子线程
        {
            
            byte[] data = new byte[2048];//byte数据类型，用于保存接受的socket数据
            newtcpclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//实例化socket对象
            try
            {
                IPEndPoint ie = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));//设置ip地址与端口号
                try
                {
                    newtcpclient.Connect(ie);//开始连接
                }
                catch (SocketException err)
                {
                    Updatetext("与服务器无法建立连接！请确保服务端已经开启，且IP与端口都输入无误\n自动退出中......");//抛出异常
                    //Updatetext(err.ToString());
                    flag = 1;//设置网络标志位1，也就是无法连接到网络
                    Thread.Sleep(3000);
                    this.Dispose();
                    this.Close();
                    f1.Show();
                    return;
                }
            }
            catch (FormatException)
            {
                try
                {
                    ipdomain = Dns.GetHostAddresses(ip)[0];//DNS解析域名
                    IPEndPoint ie = new IPEndPoint(ipdomain, int.Parse(port));//设置ip地址与端口号
                    //try
                    //{
                    newtcpclient.Connect(ie);//开始连接
                    //}
                }
                catch (SocketException err)
                {
                    Updatetext("与服务器无法建立连接！请确保服务端已经开启，且IP与端口都输入无误\n自动退出中......");//抛出异常
                    //MessageBox.Show(err.ToString());
                    flag = 1;//设置网络标志位1，也就是无法连接到网络
                    Thread.Sleep(3000);
                    this.Dispose();
                    this.Close();
                    f1.Show();
                    return;
                }
            }
            byte[] temp;
            temp = CombomBinaryArray(BitConverter.GetBytes(OSHelper.IsUnix), Encoding.UTF8.GetBytes(name.ToCharArray()));
            newtcpclient.Send(temp);//将判断是否为unix系统的结果和用户名发送给服务端
            int recv = newtcpclient.Receive(data);//接收服务器上线数据
            //MessageBox.Show(recv.ToString());
            string stringdata = Encoding.UTF8.GetString(data, 0, recv);//将byte数据转化为字符类型
            Updatetext(stringdata);//传入ui数据并刷新
            this.richTextBox1.AppendText("\n");
            while (true)
            {
                data = new byte[2048];//byte数据类型，用于保存接受的socket数据
                try
                {
                    recv = newtcpclient.Receive(data);//接收消息数据
                    //MessageBox.Show(recv.ToString());
                    stringdata = Encoding.UTF8.GetString(data, 0, recv);//将byte数据转化为字符类型
                    Updatetext(stringdata);//传入ui数据刷新
                }
                catch (SocketException s) {
                    this.Dispose();
                    this.Close();
                    //MessageBox.Show(thread.IsAlive.ToString());
                    f1.Show();
                    break;
                }
            }

        }

        private void Updatetext(String messg)//ui刷新函数，传入值为消息
        {
            this.richTextBox1.Update(); //.BeginUpdate();//开始刷新
            RichTextBox lvi=new RichTextBox();
            lvi.Text = messg;//设置item对象文本值
            this.richTextBox1.AppendText(lvi.Text);//Add(lvi);//添加item对象
            //this.richrichTextBox2. //.EndUpdate();//停止刷新
            //this.richrichTextBox2.Focus();
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)//点击窗口x调用该函数
        {
            if (flag==0) { //判断网络标志是否为0，也就是是否成功连接到网络。
            newtcpclient.Send(Encoding.UTF8.GetBytes("exitthis"));//设置字符编码为UTF-8并发送退出消息
            newtcpclient.Shutdown(SocketShutdown.Both);//停止socket连接
            newtcpclient.Close();//关闭socket连接
            }
            thread.Abort();//关闭子线程
            f1.Show();//显示登录窗口form1
        }

        private void richrichTextBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void button1_Click(object sender, EventArgs e)//点击发送按钮调用该函数
        {
            if (!richTextBox2.Text.Equals(""))//判断输入的消息是否为空
            {
                if (flag == 0)//判断网络连接是否成功
                {
                    //for(int i=0;i<richTextBox2.Rtf.Length;i++)
                    if(richTextBox2.Rtf.IndexOf(@"{\pict\") > -1){
                        pictidx[0]=richTextBox2.Rtf.IndexOf(@"{\pict\");
                        MessageBox.Show(String.Format("图片位置为{0}",pictidx[0].ToString()) );}
                    newtcpclient.Send(Encoding.UTF8.GetBytes("\n"+name + ":\n" + richTextBox2.Text + "\n"));//发送socket消息，并编码UTF-8
                    richTextBox2.Text = "";//清空发送栏

                    pictobyte.Clear();//清空表情转字节流的缓存区
                    richTextBox2.Focus();//将光标聚焦在输入框
                }
            }
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (comboBox1.Text == "按Enter发送")
            {
                if (keyData == Keys.Enter && !this.button1.Focused)
                {
                    button1_Click(null, null);//点击发送键
                    return true;//返回 true 以指示它已处理该键
                }
            }
            if (comboBox1.Text == "按Ctrl+Enter发送")
            {
                if (keyData == (Keys.Control | Keys.Enter) && !this.button1.Focused)
                {
                    button1_Click(null, null);//点击发送键
                    return true;//返回 true 以指示它已处理该键
                }
            }
            return base.ProcessDialogKey(keyData);
        }
        public class OSHelper
        {
            
            public static bool IsUnix
            {
                get
                {
                    return Environment.OSVersion.Platform == PlatformID.Unix;//返回当前系统是否是unix系统
                }
            }
        }
        private byte[] CombomBinaryArray(byte[] srcArray1, byte[] srcArray2)
        {
            byte[] newArray = new byte[srcArray1.Length + srcArray2.Length];
            Array.Copy(srcArray1, 0, newArray, 0, srcArray1.Length);
            Array.Copy(srcArray2, 0, newArray, srcArray1.Length, srcArray2.Length);
            return newArray;
        }
        private Byte[] BmpConvertByte(Image image)
        {
            MemoryStream ms1 = new MemoryStream();
            image.Save(ms1, System.Drawing.Imaging.ImageFormat.Bmp);
            return ms1.GetBuffer();
        }
        public System.Drawing.Image ReturnPhoto(byte[] streamByte)
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
        private void InitImageControl(int colCount, int rowCount)
        {
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    PictureBox picBox = new PictureBox();
                    picBox.SizeMode = PictureBoxSizeMode.CenterImage;
                    int emojinumber = (i + 1) * (j + 1);
                    string tempath = "bq__" + emojinumber.ToString() + "_";
                    try{
                        ResourceManager rm = new ResourceManager(typeof(Properties.Resources));//读取Resource文件中嵌入的图片文件
                        picBox.Image= (Bitmap)rm.GetObject("bq__"+emojinumber.ToString() + "_");
                    }
                    catch(Exception e){
                        MessageBox.Show("异常" + e.Message);
                    }
                    Size controlSize = new Size(32, 32);
                    picBox.Size = controlSize;
                    picBox.Click += new System.EventHandler(pictureBox_Click);
                    int controlLoctionX = controlSize.Width * j;
                    int controlLoctionY = controlSize.Height * i;
                    picBox.Location = new Point(controlLoctionX, controlLoctionY);
                    //picBox.MouseHover += new EventHandler(picBox_MouseHover);
                    panel1.Controls.Add(picBox);
                    
                }
            }
        }
        //当前操作系统是否为简体中文  
        public static bool IsChineseSimple()
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.Name == "zh-CN";
        }
        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;//定义鼠标当前点击picureBox的行为
            if (pic == null)
            {
                return;
            }
            Image clickemoji = pic.Image;
            pictobyte.Add(BmpConvertByte(clickemoji));
            Clipboard.SetDataObject(clickemoji);
            richTextBox2.Paste(DataFormats.GetFormat(DataFormats.Bitmap));
            
            Clipboard.Clear();
            //richTextBox2.AppendText(clickemoji.ToString());
            ////string tag = pic.Tag.ToString();
            //string tag = pic.Created.ToString();
            //MessageBox.Show(tag);//显示每一个图片位置编号,其他的功能根据自己需要扩展
            ////this.groupBox.Refresh();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (panel1.Visible)
                panel1.Visible = false;//隐藏表情框
            else
            {
                panel1.Visible = true;//显示表情框
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //初始化一个OpenFileDialog类 
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "(*.png)|*.png|(*.jpg)|*.jpg|(*.jpeg)|*.jpeg";
            //判断用户是否正确的选择了文件 
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                //获取用户选择文件的后缀名 
                string extension = Path.GetExtension(fileDialog.FileName);
                //声明允许的后缀名 

                string[] str = new string[] { ".png", ".jpeg", ".jpg" };
                if (!str.Contains(extension))
                {
                    MessageBox.Show("仅能上传png/jpeg/jpg格式的图片！");
                }
                else
                {
                    //获取用户选择的文件，并判断文件大小不能超过20K，fileInfo.Length是以字节为单位的 
                    FileInfo fileInfo = new FileInfo(fileDialog.FileName);
                    if (fileInfo.Length > 2048000)
                    {
                        MessageBox.Show("上传的图片不能大于2000K");
                    }
                    else//在这里就可以写获取到正确文件后的代码了
                    {
                        string filepath = Path.GetFullPath(fileDialog.FileName);
                        byte[] bytes = GetPictureBytes(filepath); //图片路径，转成字节流  
                        MessageBox.Show("创建字节流成功"+filepath);
                        //client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//实例化Socket对象  
                        //client.Connect(iep);//与该ip地址进行连接  
                        byte[] datasize = new byte[4];
                        datasize = BitConverter.GetBytes(bytes.Length);    //把长度作为16进制数放在datasize中  
                        newtcpclient.Send(datasize);      //发送字节流长度给服务器  
                        newtcpclient.Send(bytes, bytes.Length, SocketFlags.None);  //发送图片字节 
                         
                    }
                }
            } 

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
