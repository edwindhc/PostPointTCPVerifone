using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using WinPOSPointLib;
using System.Text.RegularExpressions;

namespace PosPointCommunication
{
    public partial class Form1 : Form
    {
        TcpClient clientSocket = new TcpClient();
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;
        WinPOSPoint wpp = new WinPOSPoint();
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "127.0.0.1";
            textBox2.Text = "23";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            clientSocket.Connect(textBox1.Text, Int32.Parse(textBox2.Text));
            Thread ctThread = new Thread(getMessage);
            ctThread.Start();

        }

        private void getMessage()
        {
            string returnData;
            while(true)
            {
                serverStream = clientSocket.GetStream();
                var buffSize = clientSocket.ReceiveBufferSize;
                byte[] inStream = new byte[buffSize];
                serverStream.Read(inStream, 0, buffSize);
                returnData = System.Text.Encoding.ASCII.GetString(inStream);
                readData = returnData;
                msg();
            }
        }

        private void msg()
        {
            string data = Regex.Replace(readData, "[ ]", "");
            string programVersion;
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(msg));
            }
            else
            {
                textBox4.Text = readData;
                if (textBox4.Text == "ProgramVersion")
                {
                    programVersion = wpp.ProgramVersion;
                    textBox3.Text = programVersion;
                    byte[] outStream = Encoding.ASCII.GetBytes(programVersion);
                    serverStream.Write(outStream, 0, outStream.Length);
                    serverStream.Flush();
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            byte[] outStream = Encoding.ASCII.GetBytes(textBox3.Text);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }
    }
}
