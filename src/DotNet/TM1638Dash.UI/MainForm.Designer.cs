namespace TM1638Dash
{
    partial class MainForm
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
            this.cmbComPort = new System.Windows.Forms.ComboBox();
            this.lblComPort = new System.Windows.Forms.Label();
            this.btnRefreshCom = new System.Windows.Forms.Button();
            this.btnConnectDevice = new System.Windows.Forms.Button();
            this.btnConnectGame = new System.Windows.Forms.Button();
            this.btnDisconnectDevice = new System.Windows.Forms.Button();
            this.btnDisconnectGame = new System.Windows.Forms.Button();
            this.rdoImperial = new System.Windows.Forms.RadioButton();
            this.rdoMetric = new System.Windows.Forms.RadioButton();
            this.lblStyle = new System.Windows.Forms.Label();
            this.cmbStyle = new System.Windows.Forms.ComboBox();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // cmbComPort
            // 
            this.cmbComPort.FormattingEnabled = true;
            this.cmbComPort.Location = new System.Drawing.Point(74, 12);
            this.cmbComPort.Name = "cmbComPort";
            this.cmbComPort.Size = new System.Drawing.Size(121, 21);
            this.cmbComPort.TabIndex = 0;
            // 
            // lblComPort
            // 
            this.lblComPort.AutoSize = true;
            this.lblComPort.Location = new System.Drawing.Point(12, 15);
            this.lblComPort.Name = "lblComPort";
            this.lblComPort.Size = new System.Drawing.Size(56, 13);
            this.lblComPort.TabIndex = 1;
            this.lblComPort.Text = "COM Port:";
            // 
            // btnRefreshCom
            // 
            this.btnRefreshCom.Location = new System.Drawing.Point(201, 12);
            this.btnRefreshCom.Name = "btnRefreshCom";
            this.btnRefreshCom.Size = new System.Drawing.Size(71, 22);
            this.btnRefreshCom.TabIndex = 2;
            this.btnRefreshCom.Text = "Refresh";
            this.btnRefreshCom.UseVisualStyleBackColor = true;
            this.btnRefreshCom.Click += new System.EventHandler(this.btnRefreshCom_Click);
            // 
            // btnConnectDevice
            // 
            this.btnConnectDevice.Location = new System.Drawing.Point(12, 62);
            this.btnConnectDevice.Name = "btnConnectDevice";
            this.btnConnectDevice.Size = new System.Drawing.Size(127, 23);
            this.btnConnectDevice.TabIndex = 3;
            this.btnConnectDevice.Text = "Connect Device";
            this.btnConnectDevice.UseVisualStyleBackColor = true;
            this.btnConnectDevice.Click += new System.EventHandler(this.btnConnectDevice_Click);
            // 
            // btnConnectGame
            // 
            this.btnConnectGame.Location = new System.Drawing.Point(145, 62);
            this.btnConnectGame.Name = "btnConnectGame";
            this.btnConnectGame.Size = new System.Drawing.Size(127, 23);
            this.btnConnectGame.TabIndex = 4;
            this.btnConnectGame.Text = "Connect Game";
            this.btnConnectGame.UseVisualStyleBackColor = true;
            this.btnConnectGame.Click += new System.EventHandler(this.btnConnectGame_Click);
            // 
            // btnDisconnectDevice
            // 
            this.btnDisconnectDevice.Location = new System.Drawing.Point(12, 62);
            this.btnDisconnectDevice.Name = "btnDisconnectDevice";
            this.btnDisconnectDevice.Size = new System.Drawing.Size(127, 23);
            this.btnDisconnectDevice.TabIndex = 6;
            this.btnDisconnectDevice.Text = "Disconnect Device";
            this.btnDisconnectDevice.UseVisualStyleBackColor = true;
            this.btnDisconnectDevice.Visible = false;
            this.btnDisconnectDevice.Click += new System.EventHandler(this.btnDisconnectDevice_Click);
            // 
            // btnDisconnectGame
            // 
            this.btnDisconnectGame.Location = new System.Drawing.Point(145, 62);
            this.btnDisconnectGame.Name = "btnDisconnectGame";
            this.btnDisconnectGame.Size = new System.Drawing.Size(127, 23);
            this.btnDisconnectGame.TabIndex = 7;
            this.btnDisconnectGame.Text = "Disconnect Game";
            this.btnDisconnectGame.UseVisualStyleBackColor = true;
            this.btnDisconnectGame.Visible = false;
            this.btnDisconnectGame.Click += new System.EventHandler(this.btnDisconnectGame_Click);
            // 
            // rdoImperial
            // 
            this.rdoImperial.AutoSize = true;
            this.rdoImperial.Checked = true;
            this.rdoImperial.Location = new System.Drawing.Point(15, 39);
            this.rdoImperial.Name = "rdoImperial";
            this.rdoImperial.Size = new System.Drawing.Size(61, 17);
            this.rdoImperial.TabIndex = 8;
            this.rdoImperial.TabStop = true;
            this.rdoImperial.Text = "Imperial";
            this.rdoImperial.UseVisualStyleBackColor = true;
            // 
            // rdoMetric
            // 
            this.rdoMetric.AutoSize = true;
            this.rdoMetric.Location = new System.Drawing.Point(82, 39);
            this.rdoMetric.Name = "rdoMetric";
            this.rdoMetric.Size = new System.Drawing.Size(54, 17);
            this.rdoMetric.TabIndex = 9;
            this.rdoMetric.Text = "Metric";
            this.rdoMetric.UseVisualStyleBackColor = true;
            // 
            // lblStyle
            // 
            this.lblStyle.AutoSize = true;
            this.lblStyle.Location = new System.Drawing.Point(142, 41);
            this.lblStyle.Name = "lblStyle";
            this.lblStyle.Size = new System.Drawing.Size(33, 13);
            this.lblStyle.TabIndex = 10;
            this.lblStyle.Text = "Style:";
            // 
            // cmbStyle
            // 
            this.cmbStyle.FormattingEnabled = true;
            this.cmbStyle.Location = new System.Drawing.Point(181, 38);
            this.cmbStyle.Name = "cmbStyle";
            this.cmbStyle.Size = new System.Drawing.Size(91, 21);
            this.cmbStyle.TabIndex = 11;
            // 
            // lbLog
            // 
            this.lbLog.FormattingEnabled = true;
            this.lbLog.Location = new System.Drawing.Point(12, 104);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(260, 160);
            this.lbLog.TabIndex = 12;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 281);
            this.Controls.Add(this.lbLog);
            this.Controls.Add(this.cmbStyle);
            this.Controls.Add(this.lblStyle);
            this.Controls.Add(this.rdoMetric);
            this.Controls.Add(this.rdoImperial);
            this.Controls.Add(this.btnDisconnectGame);
            this.Controls.Add(this.btnDisconnectDevice);
            this.Controls.Add(this.btnConnectGame);
            this.Controls.Add(this.btnConnectDevice);
            this.Controls.Add(this.btnRefreshCom);
            this.Controls.Add(this.lblComPort);
            this.Controls.Add(this.cmbComPort);
            this.Name = "MainForm";
            this.Text = "TM1638Dash";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbComPort;
        private System.Windows.Forms.Label lblComPort;
        private System.Windows.Forms.Button btnRefreshCom;
        private System.Windows.Forms.Button btnConnectDevice;
        private System.Windows.Forms.Button btnConnectGame;
        private System.Windows.Forms.Button btnDisconnectDevice;
        private System.Windows.Forms.Button btnDisconnectGame;
        private System.Windows.Forms.RadioButton rdoImperial;
        private System.Windows.Forms.RadioButton rdoMetric;
        private System.Windows.Forms.Label lblStyle;
        private System.Windows.Forms.ComboBox cmbStyle;
        private System.Windows.Forms.ListBox lbLog;
    }
}

