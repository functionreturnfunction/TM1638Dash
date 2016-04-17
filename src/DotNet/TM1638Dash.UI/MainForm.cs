using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace TM1638Dash
{
    public partial class MainForm : Form
    {
        public const LEDStyle DEFAULT_LED_STYLE = LEDStyle.Left;

        #region Private Members

        private readonly IUsbDeviceHelper _device;
        private readonly IAssettoCorsaHelper _game;

        #endregion

        #region Constructors

        public MainForm(IUsbDeviceHelper device, IAssettoCorsaHelper game)
        {
            InitializeComponent();
            _device = device;
            _game = game;
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

        #region Form Events

        private void MainForm_Load(object sender, EventArgs e)
        {
            ListComPorts();
            cmbStyle.DataSource = Enum.GetValues(typeof(LEDStyle));
            cmbStyle.SelectedItem = DEFAULT_LED_STYLE;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_device.Started)
            {
                _device.Stop();
            }
            if (_game.Started)
            {
                _game.Stop();
            }
        }

        private void btnRefreshCom_Click(object sender, EventArgs e)
        {
            ListComPorts();
        }

        #endregion

        #region Connect/Disconnect Buttons

        private void btnConnectDevice_Click(object sender, EventArgs e)
        {
            if (cmbComPort.SelectedItem != null)
            {
                _device.Start(cmbComPort.SelectedItem.ToString());
                cmbComPort.Enabled = btnRefreshCom.Enabled = btnConnectDevice.Visible = false;
                btnDisconnectDevice.Visible = true;
            }
            else
            {
                MessageBox.Show("Please select a COM port.");
            }
        }

        private void btnDisconnectDevice_Click(object sender, EventArgs e)
        {
            _device.Stop();
            btnDisconnectDevice.Visible = false;
            cmbComPort.Enabled = btnRefreshCom.Enabled = btnConnectDevice.Visible = true;
        }

        private void btnConnectGame_Click(object sender, EventArgs e)
        {
            _game.Start(rdoImperial.Checked, (LEDStyle)Enum.Parse(typeof(LEDStyle), cmbStyle.SelectedValue.ToString()));
            rdoImperial.Enabled = rdoMetric.Enabled = cmbStyle.Enabled = btnConnectGame.Visible = false;
            btnDisconnectGame.Visible = true;
        }

        private void btnDisconnectGame_Click(object sender, EventArgs e)
        {
            _game.Stop();
            btnDisconnectGame.Visible = false;
            rdoImperial.Enabled = rdoMetric.Enabled = cmbStyle.Enabled = btnConnectGame.Visible = true;
        }

        #endregion
    }
}
