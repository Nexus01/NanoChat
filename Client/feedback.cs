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
    public partial class feedback : Form
    {
        public feedback()
        {
            InitializeComponent();
            //webBrowser1.Navigate("https://github.com/Nexus01/NanoChat/issues/new");
            //this.webBrowser1.ScriptErrorsSuppressed = true;
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Nexus01/NanoChat/issues/new");
        }
    }
}
