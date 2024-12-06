using System;
using System.Windows.Forms;

namespace COM_Tester
{
    // Developed by Tin Maung Htay.
    // scsfdev@gmail.com
    // Version 1.0.3.

    public partial class frmQMTester : Form
    {
        string title = "QM30 Tester";
        int iCounter = 0;

        public frmQMTester()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            iCounter = 0;
            lblCount.Text = "-";
            txt.Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
                return;

            string strNum = "0123456789";
            if (!strNum.Contains(e.KeyChar.ToString()))
                e.Handled = true;
        }

        private void frmQMTester_Load(object sender, EventArgs e)
        {
            sr.DataReceived += Sr_DataReceived;

            cboRate.SelectedIndex = 5;
            sr.PortName = "COM" + txtCOM.Text.Trim();
            sr.BaudRate = int.Parse(cboRate.Text);

            lblCount.Text = "-";
            txt.SelectAll();
            txt.Focus();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (txtCOM.Text.Trim() == "" || cboRate.SelectedIndex < 0)
                MessageBox.Show("Please input COM port and select Baud rate info!", title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                int iCom = 0;

                try
                {
                    iCom = int.Parse(txtCOM.Text);
                }
                catch
                {
                    MessageBox.Show("COM port must be numeric!", title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (iCom <= 0)
                {
                    MessageBox.Show("COM port cannot be zero!", title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                // Start reading.

                if (sr.IsOpen == false)
                {
                    sr.PortName = "COM" + txtCOM.Text.Trim();
                    sr.BaudRate = int.Parse(cboRate.Text);
                }

                iCounter = 0;
                lblCount.Text = "-";
                txt.Text = "";

                try
                {
                    sr.Open();
                    sr.ReadTimeout = 3000;

                    sr.DiscardInBuffer();
                    sr.DiscardOutBuffer();

                    cboRate.Enabled = false;
                    txtCOM.Enabled = false;
                    btnStart.Enabled = false;
                    btnStop.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fail to open COM port!", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCOM.Focus();
                }
            }
        }

        private void Sr_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                string readData = sr.ReadTo("\r");

                if (txt.InvokeRequired == true)
                    Invoke(new MethodInvoker(delegate
                    {
                        iCounter++;
                        txt.Text += readData + Environment.NewLine;
                        txt.SelectionLength = txt.Text.Length;
                        txt.ScrollToCaret();
                    }));
                else
                {
                    iCounter++;
                    txt.Text += readData + Environment.NewLine;
                    txt.SelectionLength = txt.Text.Length;
                    txt.ScrollToCaret();
                }

                if (lblCount.InvokeRequired == true)
                    Invoke(new MethodInvoker(delegate { lblCount.Text = iCounter.ToString(); }));
                else
                    lblCount.Text = iCounter.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Read timeout!" + Environment.NewLine + "Please make sure the Baud rate is correct. Default: 38400", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmQMTester_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sr.IsOpen)
            {
                sr.DiscardInBuffer();
                sr.DiscardOutBuffer();
                sr.Close();
                sr.Dispose();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (sr.IsOpen)
            {
                sr.DiscardInBuffer();
                sr.DiscardOutBuffer();
                sr.Close();

                cboRate.Enabled = true;
                txtCOM.Enabled = true;
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
        }
    }
}
