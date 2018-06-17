using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace NanoChat
{
    public partial class config : Form
    {
        Form frm1;
        public config(Form frm1)
        {
            this.frm1 = frm1;
            
            InitializeComponent();
            using(StreamReader configreader=new StreamReader(Netconfig.fileinfo.ToString()))
            {
            this.textBox1.Text = configreader.ReadLine();
            this.textBox2.Text = configreader.ReadLine();
            }
        }
        private void config_FormClosed(object sender, FormClosedEventArgs e)
        {
            settings fr = new settings(frm1);
            fr = (settings)this.Owner;
            fr.Dispose();
            fr.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string defaultip = textBox1.Text;
            string defaultport = textBox2.Text;
            IPAddress defaultdomain;
            using (StreamWriter configwriter = new StreamWriter(Netconfig.fileinfo.ToString()))
            {
                try
                {
                    IPAddress ipTry = IPAddress.Parse(defaultip);
                    Netconfig.crossip = ipTry.ToString();
                    configwriter.WriteLine(Netconfig.crossip);
                    if (int.Parse(defaultport) >= 1025 && int.Parse(defaultport) <= 65535)
                    {
                        Netconfig.crossip = defaultport;
                        configwriter.WriteLine(Netconfig.crossip);
                    }
                    else
                    {
                        configwriter.WriteLine(Netconfig.crossport);
                        MessageBox.Show("请确保端口在1025～65535之间");
                    }
                }
                catch (FormatException f)
                {
                    try
                    {
                        defaultdomain = Dns.GetHostAddresses(defaultip)[0];
                        Netconfig.crossip = defaultip;
                        configwriter.WriteLine(Netconfig.crossip);
                        //Netconfig.crossip = defaultdomain.ToString();
                        if (int.Parse(defaultport) >= 1025 && int.Parse(defaultport) <= 65535)
                        {
                            Netconfig.crossport = defaultport;
                            configwriter.WriteLine(Netconfig.crossport);
                        }
                        else
                        {
                            configwriter.WriteLine(Netconfig.crossport);
                            MessageBox.Show("请确保端口在1025～65535之间");
                        }
                    }
                    catch (Exception excp)
                    {
                        Netconfig.crossip = defaultip;
                        configwriter.WriteLine(Netconfig.crossip);
                        Netconfig.crossport = defaultport;
                        configwriter.WriteLine(Netconfig.crossport);
                        MessageBox.Show(excp.Message);
                    }
                }
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //using (StreamWriter configwriter = new StreamWriter(Netconfig.fileinfo.ToString()))
            //{
            //    configwriter.WriteLine("127.0.0.1");
            //    configwriter.WriteLine("2018");
            //}
            Netconfig.fileinfo.Delete();
            MessageBox.Show("重置成功，重启生效");
        }
    }
}
