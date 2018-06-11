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
            this.webBrowser1.Navigate("https://www.zhihu.com");
            this.webBrowser1.ScriptErrorsSuppressed = true;
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //webBrowser1.Navigate("https://github.com/Nexus01/NanoChat/issues/new");
        }
    }
}
