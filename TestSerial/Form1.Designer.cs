namespace TestSerial
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtResult = new System.Windows.Forms.TextBox();
            this.sr = new System.IO.Ports.SerialPort(this.components);
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.txtCOM = new System.Windows.Forms.TextBox();
            this.txtBaud = new System.Windows.Forms.TextBox();
            this.txtMS = new System.Windows.Forms.TextBox();
            this.tmr = new System.Windows.Forms.Timer(this.components);
            this.btnStop2 = new System.Windows.Forms.Button();
            this.btnStart2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(12, 68);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(260, 215);
            this.txtResult.TabIndex = 4;
            // 
            // sr
            // 
            this.sr.BaudRate = 38400;
            this.sr.PortName = "COM4";
            this.sr.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.sr_DataReceived);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 38);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "START";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Visible = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(105, 38);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "STOP";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Visible = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(197, 38);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "EXIT";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Visible = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtCOM
            // 
            this.txtCOM.Location = new System.Drawing.Point(13, 13);
            this.txtCOM.Name = "txtCOM";
            this.txtCOM.Size = new System.Drawing.Size(58, 20);
            this.txtCOM.TabIndex = 0;
            this.txtCOM.Text = "COM4";
            // 
            // txtBaud
            // 
            this.txtBaud.Location = new System.Drawing.Point(77, 12);
            this.txtBaud.Name = "txtBaud";
            this.txtBaud.Size = new System.Drawing.Size(58, 20);
            this.txtBaud.TabIndex = 1;
            this.txtBaud.Text = "38400";
            // 
            // txtMS
            // 
            this.txtMS.Location = new System.Drawing.Point(197, 12);
            this.txtMS.MaxLength = 4;
            this.txtMS.Name = "txtMS";
            this.txtMS.Size = new System.Drawing.Size(58, 20);
            this.txtMS.TabIndex = 5;
            this.txtMS.Text = "5";
            this.txtMS.Visible = false;
            // 
            // tmr
            // 
            this.tmr.Interval = 3000;
            this.tmr.Tick += new System.EventHandler(this.tmr_Tick);
            // 
            // btnStop2
            // 
            this.btnStop2.Location = new System.Drawing.Point(105, 38);
            this.btnStop2.Name = "btnStop2";
            this.btnStop2.Size = new System.Drawing.Size(75, 23);
            this.btnStop2.TabIndex = 3;
            this.btnStop2.Text = "STOP";
            this.btnStop2.UseVisualStyleBackColor = true;
            this.btnStop2.Click += new System.EventHandler(this.btnStop2_Click);
            // 
            // btnStart2
            // 
            this.btnStart2.Location = new System.Drawing.Point(12, 38);
            this.btnStart2.Name = "btnStart2";
            this.btnStart2.Size = new System.Drawing.Size(75, 23);
            this.btnStart2.TabIndex = 2;
            this.btnStart2.Text = "START";
            this.btnStart2.UseVisualStyleBackColor = true;
            this.btnStart2.Click += new System.EventHandler(this.btnStart2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 298);
            this.Controls.Add(this.btnStop2);
            this.Controls.Add(this.btnStart2);
            this.Controls.Add(this.txtMS);
            this.Controls.Add(this.txtBaud);
            this.Controls.Add(this.txtCOM);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtResult);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Test Serial";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtResult;
        private System.IO.Ports.SerialPort sr;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtCOM;
        private System.Windows.Forms.TextBox txtBaud;
        private System.Windows.Forms.TextBox txtMS;
        private System.Windows.Forms.Timer tmr;
        private System.Windows.Forms.Button btnStop2;
        private System.Windows.Forms.Button btnStart2;
    }
}

