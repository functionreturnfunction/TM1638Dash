using System;
using System.IO.Ports;
using System.Windows.Forms;
using AssettoCorsaSharedMemory;

namespace SimDash.UI
{
    public partial class MainForm : Form
    {
        #region Private Members

        private UsbDevice _device;
        private AssettoCorsa _game;
        private int _currentMaxRpm;

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
            _game?.Stop();
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
            _game.Stop();
            _game.PhysicsUpdated -= OnPhysicsUpdated;
            _game.StaticInfoUpdated -= OnStaticInfoUpdated;
            btnDisconnectGame.Visible = false;
            btnConnectGame.Visible = true;
        }

        private void OnStaticInfoUpdated(object sender, StaticInfoEventArgs e)
        {
            _currentMaxRpm = e.StaticInfo.MaxRpm;
        }

        private void btnConnectGame_Click(object sender, EventArgs e)
        {
            _game = new AssettoCorsa();
            _game.PhysicsUpdated += OnPhysicsUpdated;
            _game.StaticInfoUpdated += OnStaticInfoUpdated;
            _game.Start();
            btnConnectGame.Visible = false;
            btnDisconnectGame.Visible = true;
        }

        private void OnPhysicsUpdated(object sender, PhysicsEventArgs e)
        {
            if (_device != null && _device.Connected)
            {
                _device.SendString(BuildCommandString(e));
            }
        }

        private string BuildCommandString(PhysicsEventArgs e)
        {
            return DetermineLights(e.Physics.Rpms) + "00" + FixGear(e.Physics.Gear) + FixSpeed(e.Physics.SpeedKmh);
        }

        private int ScaleRPMs(int percentage)
        {
            return 1 + (percentage - 61)/(90 - 61)*7;
        }

        private string DetermineLights(int rpms)
        {
            if (rpms == 0 || _currentMaxRpm == 0)
            {
                return "00";
            }

            var percentage = rpms * 100.0 / _currentMaxRpm;

            if (percentage < 60)
            {
                return "00";
            }

            var scaled = ScaleRPMs((int)Math.Round(percentage));

            return (scaled ^ 2).ToString("X").PadLeft(2, '0');
        }

        private static string FixGear(int gear)
        {
            switch (gear)
            {
                case 0:
                    return "r";
                case 1:
                    return "n";
                default:
                    return (gear - 1).ToString();
            }
        }

        private static string FixSpeed(float speedKmh)
        {
            return Math.Round(speedKmh * 0.621371192).ToString().PadLeft(7, ' ');
        }

        #endregion
    }
}
