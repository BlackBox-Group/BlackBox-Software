using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace BlackBox
{
    public partial class Form1 : Form
    {
        bool isConnected = false;
        String[] ports;
        SerialPort port;
        Thread serialThread;

        // String serialCommand;

        ConnectRFIDForm openRFIDform;
        CardexistForm openCardexistForm;
        

        public Form1()
        {
            InitializeComponent();
            disableControls();
            getAvailableComPorts();

            openRFIDform = new ConnectRFIDForm();
            openRFIDform.StartPosition = FormStartPosition.CenterScreen;
            openCardexistForm = new CardexistForm();
            openCardexistForm.StartPosition = FormStartPosition.CenterScreen;

            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
                Console.WriteLine(port);
                if (ports[0] != null)
                {
                    comboBox1.SelectedItem = ports[0];
                }
            }
        }

        ~Form1()
        {
            disconnectFromArduino();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                connectToArduino();
            }
            else
            {
                disconnectFromArduino();
            }
        }
        void getAvailableComPorts()
        {
            ports = SerialPort.GetPortNames();
        }
        private void connectToArduino()
        {
            isConnected = true;
            string selectedPort = comboBox1.GetItemText(comboBox1.SelectedItem);
            port = new SerialPort(selectedPort, 9600, Parity.None, 8, StopBits.One);
            port.Open();
            port.Write("#START\n");
            button1.Text = "Disconnect";
            enableControls();

            serialThread = new Thread(checkSerial);
            serialThread.Start();
        }

        private void disconnectFromArduino()
        {
            isConnected = false;
            port.Write("#STOP\n");
            port.Close();
            button1.Text = "Connect";
            disableControls();
        }
        private void enableControls()
        {
            groupBox1.Enabled = true;
        }
        private void disableControls()
        {
            groupBox1.Enabled = false;
        }

        private void checkSerial()
        {
            while (true)
            {
                Console.WriteLine("Avaiting serial line...");
                String serialCommand = port.ReadLine();
                Console.WriteLine($"Got: \"{serialCommand}\"");
                
                if (!isConnected) break;
                Action a = () => analyzeCommand(serialCommand);
                if (InvokeRequired)
                    Invoke(a);
                else
                    a();
            }
        }

        private void analyzeCommand(String serialCommand) {
            Console.WriteLine("Test1");
            if (serialCommand.StartsWith("#")) return;
            Console.WriteLine("Test2");

            if (serialCommand == "putRFID")
            {
                Console.WriteLine("Test3");
                openRFIDform.Show();
            }
            else if (serialCommand == "username?")
            {
                openRFIDform.Close();
                UsernameForm openUsernameForm = new UsernameForm();
                openUsernameForm.StartPosition = FormStartPosition.CenterScreen;
                openUsernameForm.Show();
            }
            else if (serialCommand == "cardexists")
            {
                openCardexistForm.Show();
                openRFIDform.Close();
            }
            else {
                port.Write("# wtf what is this\n");
            }

            //port.Write("username" ) - dopisat nada
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                port.Write("usercreate\n");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (isConnected)
           {
               port.Write("addService\n");

               string service = port.ReadLine();
               if (service == "service?\n")
               {
                    TitleAddForm openTitleform = new TitleAddForm();
                    openTitleform.StartPosition = FormStartPosition.CenterScreen;
                    openTitleform.Show();
                }
           }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
