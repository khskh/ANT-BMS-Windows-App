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
using System.Text.RegularExpressions;

namespace ant_bms_arduino
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        private SerialPort SerPort;
        private string ReceivedData;
        private int dataSize = 0;

        private bool is_connection_established = false;

        private byte[] data = new byte[250];


        public Form1()
        {
            InitializeComponent();
            FetchAvailablePorts();
        }

        void FetchAvailablePorts()
        {
            String[] ports = SerialPort.GetPortNames(); // Collect available port names
            AvailablePortsBox.Items.AddRange(ports); // Put port names into selection window
        }


        private void ConnectToPort_Click(object sender, EventArgs e)
        {
            if (!is_connection_established)
            {
                // predefined things
                SerPort = new SerialPort();
                SerPort.BaudRate = 19200;
                SerPort.PortName = AvailablePortsBox.Text;
                SerPort.Parity = Parity.None;
                SerPort.DataBits = 8;
                SerPort.StopBits = StopBits.One;
                SerPort.ReadBufferSize = 20000000;
                SerPort.DataReceived += SerPort_DataReceived;

                ConnectToPort.Text = "Disconnect";


                try
                {
                    SerPort.Open();         //open a port
                    Thread.Sleep(1000);     //wait a moment

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!");
                }

            } else
            {
                ConnectToPort.Text = "Connect";
                SerPort.Close();
            }
        }



        private void SerPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            ReceivedData = SerPort.ReadLine(); // Read data from port

            if (data != null)
            {
                this.Invoke(new Action(ProcessingData)); // Execute delegate (ProcessingData)
            }

        }



        private void ProcessingData()
        {
            ReceivedDataBox.Clear();
            //ReceivedDataBox.Text += ReceivedData.ToString() + Environment.NewLine;
            ReceivedDataBox.Text = ReceivedData;

            data = StringToByteArray(EnsureIsHEX(ReceivedData));

            tablica.Text = BitConverter.ToString(data).Replace("-", "");




            //voltage.Text = ((data[4] << 8) | data[5]).ToString();

            //info about array filling


            int arrayLength = data.Length;
            array_lenght.Text = arrayLength.ToString();
            
            if(data.Length >= 100)
            {
                t_voltage.Text = (((data[4] << 8) | data[5]) * 0.1).ToString("0.00");
                t_current.Text = (((data[70] << 24) | (data[71] << 16) | (data[72] << 8) | data[73]) * 0.1).ToString("0.00");
                t_soc.Text = data[74].ToString();
                t_power.Text = ((data[111] << 24) | (data[112] << 16) | (data[113] << 8) | data[114]).ToString("0.00");

                for (int i = 1; i <= 20; i++)
                {
                    int dataIndex = (i - 1) * 2 + 6; // Start from 6 and add (i-1)*2
                    double voltage = (((data[dataIndex] << 8) | data[dataIndex + 1]) * 0.001);
                    TextBox textBox = this.Controls.Find("c" + i, true).OfType<TextBox>().FirstOrDefault();

                    if (textBox != null)
                    {
                        textBox.Text = voltage.ToString("0.000") + " v";
                    }
                }
            }
        }


        // Convert a String variable containing a HEX number to a byte array
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }


        // Check if hex is in string 
        private string EnsureIsHEX(string input)
        {
            // Remove newline characters and everything after them
            input = input.Split('\n')[0];

            // Remove newline characters and everything after them
            input = input.Split('\r')[0];

            // required hexadecimal characters, checking below
            string hexPattern = "^[0-9A-Fa-f]*$";

            if (input.Length % 2 != 0)
            {
                input += "0"; // Append zero at the end if length is odd
            }

            if (Regex.IsMatch(input, hexPattern))
            {
                return input;
            }
            else
            {
                // if sth happen
                return "0000";
            }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                SerPort.Close();
            }

            catch
            {

            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void array_lenght_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
