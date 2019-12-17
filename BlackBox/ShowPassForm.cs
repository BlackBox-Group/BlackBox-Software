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
    public partial class ShowPassForm : Form
    {
        private Form1 parent;
        private String pass;
        private String passname;
        public ShowPassForm(Form1 par, string passwordL)
        {
            InitializeComponent();
            passname = passwordL;
            passwordLabel.Text = passname;
            parent = par;
        }

        public void displayPassword(string password)
        {
            pass = password;
            passwordTextBox.Text = password;
        }

        public void changeLabel(string password)
        {
            passname = password;
            passwordLabel.Text = passname;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parent.writeToSerial($"password {passname}\n");
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ShowPassForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            passwordTextBox.Text = "";
            this.Hide();
            e.Cancel = true; // this cancels the close event.
        }
    }
}
