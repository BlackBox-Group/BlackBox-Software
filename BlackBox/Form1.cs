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
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace BlackBox
{
    public partial class Form1 : Form
    {
        public bool isConnected = false;
        public string usernameText;
        public SerialPort port;

        private String[] ports;
        private Thread serialThread;

        private ConnectRFIDForm openRFIDform;
        private CardexistForm openCardexistForm;
        

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
            if (isConnected)
            {
                disconnectFromArduino();
            }
        }
        void getAvailableComPorts()
        {
            ports = SerialPort.GetPortNames();
        }
        private bool connectToArduino(string selectedPort = null)
        {
            if (isConnected) return false;
            
            if (selectedPort == null) 
            {
                selectedPort = comboBox1.GetItemText(comboBox1.SelectedItem);
            }

            port = new SerialPort
            {
                PortName = selectedPort,
                BaudRate = 9600,
                NewLine = "\r\n",
                RtsEnable = true,
                DtrEnable = true
            };

            try
            {
                port.Open();
                port.Write("#START\n");
                isConnected = true;

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to open port: ");
                Console.WriteLine(e);

                return false;
            }
        }

        private void initSerialControls() {
            button1.Text = "Disconnect";
            enableControls();

            serialThread = new Thread(checkSerial);
            serialThread.Start();

            isConnected = true;
        }

        private bool disconnectFromArduino()
        {
            if (!isConnected) return false;

            port.Write("#STOP\n");
            port.Close();
            button1.Text = "Connect";
            disableControls();

            isConnected = false;
            return true;
        }
        private void enableControls()
        {
            groupBox1.Enabled = true;
        }
        private void disableControls()
        {
            groupBox1.Enabled = false;
        }

        public bool writeToSerial(string str) {
            if (isConnected) {
                Console.WriteLine($"Sending '{str}'");
                port.Write(str);
                return true; 
            }
            
            return false;
        }

        private void checkSerial()
        {
            while (true)
            {
                Console.WriteLine("Awaiting serial line...");
                try
                {
                    String serialCommand = port.ReadLine();
                    Console.WriteLine($"Got: \"{serialCommand}\"");

                    if (!isConnected) break;
                    Action a = () => analyzeCommand(serialCommand);
                    if (InvokeRequired)
                        Invoke(a);
                    else
                        a();
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine("Got exception listening to port, oopsie");
                    break;
                }
            }
        }

        private void analyzeCommand(String serialCommand)
        {
            if (serialCommand.StartsWith("#")) return;

            if (serialCommand == "putRFID")
            {
                openRFIDform.Show();
            }
            else if (serialCommand == "username?")
            {
                openRFIDform.Hide();
                UsernameForm openUsernameForm = new UsernameForm(this);
                openUsernameForm.StartPosition = FormStartPosition.CenterScreen;
                openUsernameForm.Show();
            }
            else if (serialCommand == "userincorrect")
            {
                MessageBox.Show("The user name is incorrect!", "Error");
                UsernameForm openUsernameForm = new UsernameForm(this);
                openUsernameForm.StartPosition = FormStartPosition.CenterScreen;
                openUsernameForm.Show();
            }
            else if (serialCommand == "cardexists")
            {
                openCardexistForm.Show();
                openRFIDform.Hide();
            }
            else if (serialCommand == "masterpass?")
            {
                openRFIDform.Hide();
                MasterPassForm openMasterPassForm = new MasterPassForm(this);
                openMasterPassForm.StartPosition = FormStartPosition.CenterScreen;
                openMasterPassForm.Show();
            }
            else if (serialCommand == "usercreated")
            {
                MessageBox.Show("Success", "User has been created!");
            }
            else if (serialCommand == "nosuchcard")
            {
                MessageBox.Show("No such user", "Error");
            }
            else if (serialCommand.StartsWith("username"))
            {
                usernameText = serialCommand.Substring(9);
                // send usernameText to show in label "Welcome to the BlackBox, "
            }
            
            else if (serialCommand == "masterincorrect")
            {
                MessageBox.Show("Masterpass is incorrect!", "Error");

                MasterPassForm openMasterPassForm = new MasterPassForm(this);
                openMasterPassForm.StartPosition = FormStartPosition.CenterScreen;
                openMasterPassForm.Show();
            }
            else if (serialCommand == "service?")
            {
                openRFIDform.Hide();
                TitleAddForm openTitleform = new TitleAddForm(this);
                openTitleform.StartPosition = FormStartPosition.CenterScreen;
                openTitleform.Show();
            }
            else if (serialCommand == "servicefail")
            {
                MessageBox.Show("Something went wrong during service creation. Is your password correct?", "Oops!");
            }
            else if (serialCommand.StartsWith("service"))
            {
                string serviceTitle = serialCommand.Substring(0, serialCommand.IndexOf(':'));
                serviceTitle = serviceTitle.Replace('_', ' ');
                string serviceUrl = serialCommand.Substring(serialCommand.IndexOf(':') + 1, serialCommand.IndexOf(' '));


                ListViewItem item1 = new ListViewItem(serviceTitle);
                item1.SubItems.Add("");
                item1.SubItems.Add(serviceUrl);

                listView1.Items.AddRange(new ListViewItem[] { item1 });
            }
            /*else if (serialCommand == "userlogin")
            {
                openRFIDform.Hide();
                UsernameForm openUsernameForm = new UsernameForm(this);
                openUsernameForm.StartPosition = FormStartPosition.CenterScreen;
                openUsernameForm.Show();
            } */
            else
            {
                writeToSerial("# wtf what is this\n");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                writeToSerial("usercreate\n");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (isConnected) 
            {
               writeToSerial("addService\n");
               listView1.Items.Clear();
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

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (string portName in ports) {
                try
                {
                    Console.WriteLine($"Trying to connect to {portName}");
                    if (connectToArduino(portName))
                    {
                        Console.WriteLine("Success");
                        port.Write("ping_blackbox\n");
                        // string response = port.ReadLine();
                        // await Task.Run( () => Thread.Sleep(2000) );
                        Thread.Sleep(500);
                        Console.WriteLine("Done waiting");
    
                        Console.WriteLine(port.BytesToRead);
                        if (port.BytesToRead != 0)
                        {
                            string response = port.ReadLine();

                            Console.WriteLine(response);

                            if (response == "pong_blackbox")
                            {
                                MessageBox.Show($"Connection is set to {portName}",
                                    "Successful",
                                    MessageBoxButtons.OK);
                                Console.WriteLine("OK");
                                initSerialControls();
                                return;
                            }
                        }
                        Console.WriteLine("k1");

                    }
                    Console.WriteLine("k2");


                    disconnectFromArduino();
                    Console.WriteLine("k2");

                }
                catch (Exception ex) {
                    Console.WriteLine($"{portName} just died");
                    Console.WriteLine(ex);
                }; 
            }

            Console.WriteLine("No ports responded :(");
            MessageBox.Show("Make sure your device is connected",
                            "Unable to detect device",
                            MessageBoxButtons.OK);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                writeToSerial("userlogin\n");

            }
        }

        public void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                writeToSerial("resetcards\n");
            }
        }
    }
}
