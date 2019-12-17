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
    public partial class TitleAddForm : Form
    {
        private Form1 parent;
        private string titleText;
        private string passwordText;
        private string URLText;

        public TitleAddForm(Form1 par)
        {
            InitializeComponent();
            parent = par;
        }

        private void TitleAddForm_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            titleText = textBox1.Text;
            passwordText = textBox2.Text;
            URLText = textBox3.Text;

            titleText = titleText.Replace(' ', '_');
            parent.writeToSerial($"service {titleText}:{URLText} {passwordText}\n");
            
           if ((textBox1.Text.Length > 0) && (textBox2.Text.Length > 0) && (textBox3.Text.Length > 0))
            {
                ListViewItem item1 = new ListViewItem(textBox1.Text);
                // item1.SubItems.Add(textBox2.Text);
                item1.SubItems.Add("");
                item1.SubItems.Add(textBox3.Text);

                parent.listView1.Items.AddRange(new ListViewItem[] { item1 });
            }
            Close();
        }

        private void TitleAddForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; // this cancels the close event.
        }
    }
}
