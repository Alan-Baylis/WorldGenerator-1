using System;
using System.Net;
using System.Windows.Forms;

namespace OpenTkClient
{
    public partial class IntroForm : Form
    {
        public string ServerName { get; set; }
        public bool IsLocalServer { get { return string.Compare(ServerName, localName, ignoreCase:true) == 0; } }
        private string localName;

        public IntroForm()
        {
            localName = Dns.GetHostName();
            ServerName = localName;
            InitializeComponent();
            serverTextBox.Text = ServerName;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            ServerName = serverTextBox.Text;
            this.Close();
        }
    }
}
