using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace SimDash.UI
{
    public partial class MainForm : Form
    {
        #region Private Members

        private UsbDevice _device;

        #endregion

        #region Constructors

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void ListComPorts()
        {
            cmbComPort.Items.Clear();

            foreach (var port in SerialPort.GetPortNames())
            {
                cmbComPort.Items.Add(port);
            }
        }

        #endregion

        #region Event Handlers

        private void MainForm_Load(object sender, EventArgs e)
        {
            ListComPorts();
        }

        private void btnRefreshCom_Click(object sender, EventArgs e)
        {
            ListComPorts();
        }

        private void btnConnectDevice_Click(object sender, EventArgs e)
        {
            if (cmbComPort.SelectedItem != null)
            {
                _device = new UsbDevice(cmbComPort.SelectedItem.ToString());
                _device.SendString("0000ready   ");
                btnConnectDevice.Visible = false;
                btnDisconnectDevice.Visible = true;
            }
            else
            {
                MessageBox.Show("Please select a COM port.");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _device?.Dispose();
        }

        private void btnDisconnectDevice_Click(object sender, EventArgs e)
        {
            _device.Dispose();
            _device = null;
            btnDisconnectDevice.Visible = false;
            btnConnectDevice.Visible = true;
        }

        private void btnDisconnectGame_Click(object sender, EventArgs e)
        {
            btnDisconnectGame.Visible = false;
            btnConnectGame.Visible = true;
        }

        private void btnConnectGame_Click(object sender, EventArgs e)
        {
            btnConnectGame.Visible = false;
            btnDisconnectGame.Visible = true;
        }

        #endregion
    }
}
