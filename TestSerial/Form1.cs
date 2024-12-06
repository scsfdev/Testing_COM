using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TestSerial
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtCOM.Focus();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            sr.Close();
            sr.Dispose();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (sr.IsOpen == false)
            {
                sr.PortName = txtCOM.Text.ToUpper();
                sr.BaudRate = Int32.Parse(txtBaud.Text.Trim());
            }

            if (sr.IsOpen != false)
                sr.Close();

            Thread.Sleep(500);
            sr.Open();

            tmr.Interval = Int32.Parse(txtMS.Text.Trim()) ;
            tmr.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void sr_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (txtResult.InvokeRequired == true)
                Invoke(new MethodInvoker(delegate 
                    {
                        if (txtResult.Text.Length > 1000)
                            txtResult.Text = "";


                        //char[] result = new char[20];
                        //for (int len = 0; len < result.Length; )
                        //{
                        //    len += sr.Read(result, len, result.Length - len);
                        //}

                        //char[] result = new char[50];
                        //do
                        //{

                        //}while()

                        //txtResult.Text += sr.ReadExisting();
                        //txtResult.Text += new string(result) + Environment.NewLine;

                        txtResult.Text += sr.ReadTo("\r") + Environment.NewLine;
                        txtResult.Select(txtResult.Text.Length, 1);
                        txtResult.ScrollToCaret();
                    }));
            else
                txtResult.Text += sr.ReadExisting() + Environment.NewLine;

        //    sr.WriteLine("READOFF" + Environment.NewLine);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            sr.DiscardInBuffer();
            sr.DiscardOutBuffer();
            sr.WriteLine("READOFF" + Environment.NewLine);

            tmr.Enabled = false;
            Thread.Sleep(500);
        }

        private void tmr_Tick(object sender, EventArgs e)
        {
            //sr.WriteLine("READOFF" + Environment.NewLine);
            //sr.WriteLine("READON" + Environment.NewLine);

            // READOFF<CR>
            sr.Write(new byte[] { 0x52, 0x45, 0x41, 0x44, 0x4F, 0x46, 0x46, 0x0D }, 0, 8);
            // READON<CR>
            sr.Write(new byte[] { 0x52, 0x45, 0x41, 0x44, 0x4F, 0x4E, 0x0D }, 0, 7);
        }

        private void btnStart2_Click(object sender, EventArgs e)
        {
            if (sr.IsOpen != false)
                sr.Close();

            Thread.Sleep(500);

            if (sr.IsOpen == false)
            {
                sr.PortName = txtCOM.Text.ToUpper();
                sr.BaudRate = Int32.Parse(txtBaud.Text.Trim());
            }

            sr.Open();
            
            sr.DiscardInBuffer();
            sr.DiscardOutBuffer();
        }

        private void btnStop2_Click(object sender, EventArgs e)
        {
            sr.DiscardInBuffer();
            sr.DiscardOutBuffer();
            sr.Close();
            txtResult.Text = "";
        }
    }
}
