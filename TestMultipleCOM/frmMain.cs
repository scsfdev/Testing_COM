using System;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace TestMultipleCOM
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lst1.Items.Clear();
            lst2.Items.Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void sr1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (lst1.InvokeRequired == true)
                Invoke(new MethodInvoker(delegate
                {
                    lst1.Items.Add(sr1.ReadTo("\r"));
                    lst1.SelectedIndex = lst1.Items.Count - 1;
                }));
            else
                lst1.Items.Add(sr1.ReadExisting());
        }

        private void sr2_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (lst2.InvokeRequired == true)
                Invoke(new MethodInvoker(delegate
                {
                    lst2.Items.Add(sr2.ReadTo("\r"));
                    lst2.SelectedIndex = lst2.Items.Count - 1;
                }));
            else
                lst2.Items.Add(sr2.ReadExisting());
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            txtPort1.Text = "3";
            txtPort2.Text = "4";

            btnStart.Focus();
        }

        

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            sr1.Close();
            sr1.Dispose();

            sr2.Close();
            sr2.Dispose();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "START")
            {
                if (string.IsNullOrEmpty(txtPort1.Text) || string.IsNullOrEmpty(txtPort2.Text))
                {
                    MessageBox.Show("COM Port number cannot leave empty!", "Test Multiple COM");
                    return;
                }

                Start_Device(sr1, txtPort1.Text);
                Start_Device(sr2, txtPort2.Text);

                btnStart.Text = "STOP";

                txtPort1.ReadOnly = true;
                txtPort2.ReadOnly = true;
            }
            else
            {
                Stop_Device(sr1);
                Stop_Device(sr2);

                btnStart.Text = "START";

                txtPort1.ReadOnly = false;
                txtPort2.ReadOnly = false;
            }
        }

        private void Start_Device(SerialPort sr, string strCOM)
        {
            if (sr.IsOpen != false)
                sr.Close();

            Thread.Sleep(500);

            if (sr.IsOpen == false)
            {
                sr.PortName = "COM" + strCOM;
            }

            sr.Open();

            sr.DiscardInBuffer();
            sr.DiscardOutBuffer();
        }

        private void Stop_Device(SerialPort sr)
        {
            sr.DiscardInBuffer();
            sr.DiscardOutBuffer();
            sr.Close();
        }
    }
}
