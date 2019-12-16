using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackBox
{
    public partial class UsernameForm : Form
    {
        private Form1 parent;
        private string usernameText;
        public UsernameForm(Form1 par)
        {
            InitializeComponent();
            parent = par;
        }

        private void UsernameForm_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void button1_Click(object sender, EventArgs e)
        {
            usernameText = textBox1.Text;
            parent.writeToSerial($"username {usernameText}");
            Close();
        }
    }
}
