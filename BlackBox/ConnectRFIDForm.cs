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
    public partial class ConnectRFIDForm : Form
    {
        public ConnectRFIDForm()
        {
            InitializeComponent();
        }

        private void ConnectRFIDForm_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            label1.AutoSize = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void ConnectRFIDForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; // this cancels the close event.
        }
    }
}
