using System;
using System.Windows.Forms;

namespace RSA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void EncryptButton_Click(object sender, EventArgs e)
        {
            var cipher = CryptService.Encrypt(Input.Text);
            Output.Clear();
            Output.Text = cipher;
            Info.Clear();
            Info.Text = CryptService.GetInfo(false);
        }

        private void DecryptButton_Click(object sender, EventArgs e)
        {
            var source = CryptService.Decrypt(Input.Text);
            Output.Clear();
            Output.Text = source;
            Info.Clear();
            Info.Text = CryptService.GetInfo(true);
        }
    }
}
