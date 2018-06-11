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

namespace NanoChat
{
    public partial class config : Form
    {
        Form frm1;
        public config(Form frm1)
        {
            this.frm1 = frm1;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string defaultip = textBox1.Text;
            string defaultport = textBox2.Text;
            IPAddress defaultdomain;
            try { 
            IPAddress ipTry = IPAddress.Parse(defaultip);
            Netconfig.crossip = ipTry.ToString();
            }
            catch(FormatException f) {
                try { 
                defaultdomain=Dns.GetHostAddresses(defaultip)[0];
                Netconfig.crossip = defaultdomain.ToString();
                }
                catch(Exception excp){
                    MessageBox.Show(excp.Message);
                }
            }
            if (int.Parse(defaultport) >= 1025 && int.Parse(defaultport) <= 65535)
                Netconfig.crossport = defaultport;
            else
                MessageBox.Show("请确保端口在1025～65535之间");
            this.Close();
            //Form1 frm = new Form1(); //form1和你的要调用的那个窗体
            
            //frm1.Show();
        }
    }
}
