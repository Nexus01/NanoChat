using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NanoChat
{
    public partial class settings : Form
    {
        Form frm1;
        public settings(Form frm1)
        {
            this.frm1 = frm1;
            InitializeComponent();
            this.richTextBox1.ReadOnly = true;
            //PictureBox pb = new PictureBox();
            //pb.Image = Properties.Resources.bq__1_;
            //this.richTextBox1.Controls.Add(pb);
            //PictureBox pb1 = new PictureBox();
            //pb1.Image = Properties.Resources.bq__10_;
            //this.richTextBox1.Controls.Add(pb1);
        }
        private void settings_Load(object sender, EventArgs e)//窗口初始化函数
        {
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.settings_FormClosing);//注册窗口X事件
            //richTextBox1.AppendText(Properties.Resources.bq__1_);
            
        }
        private void settings_FormClosing(object sender, FormClosingEventArgs e)//点击窗口x调用该函数
        {

            //frm1 = new Form();
            //if (frm1.Visible == false)
            //this.Close();
            this.Hide();
            frm1.Show();
            //Application.OpenForms["Form1"].Show();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Controls.Clear();//点击按钮，先清空panel里的内容
            config form = new config(this.frm1) { TopLevel = false, FormBorderStyle = FormBorderStyle.None };//要添加的Form不显示窗体头和边框
            this.splitContainer1.Panel2.Controls.Add(form);//把form里的内容添到panel中
            form.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Show();//显示内容
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Controls.Clear();//点击按钮，先清空panel里的内容
            feedback form = new feedback { TopLevel = false, FormBorderStyle = FormBorderStyle.None };//要添加的Form不显示窗体头和边框
            this.splitContainer1.Panel2.Controls.Add(form);//把form里的内容添到panel中
            form.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Show();//显示内容
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Controls.Clear();//点击按钮，先清空panel里的内容
            about form = new about { TopLevel = false, FormBorderStyle = FormBorderStyle.None };//要添加的Form不显示窗体头和边框
            this.splitContainer1.Panel2.Controls.Add(form);//把form里的内容添到panel中
            form.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Show();//显示内容
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            frm1.Show();
        }
    }
}
