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
    public partial class MasterPassForm : Form
    {
        private Form1 parent;
        private string masterpassText;
        public MasterPassForm(Form1 par)
        {
            InitializeComponent();
            parent = par;
        }

        private void MasterPassForm_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            masterpassText = textBox1.Text;
            parent.writeToSerial($"masterpass {masterpassText}\n");
            Close();
        }

        private void MasterPassForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; // this cancels the close event.
        }
    }
}
