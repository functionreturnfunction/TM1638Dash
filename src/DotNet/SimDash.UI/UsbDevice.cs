using System;
using System.IO.Ports;

namespace SimDash.UI
{
    public class UsbDevice : IDisposable
    {
        #region Constants

        public const int BAUD = 9600;

        #endregion

        #region Private Members

        private readonly SerialPort _port;

        #endregion

        #region Constructors

        public UsbDevice(string portName)
        {
            _port = new SerialPort(portName, BAUD, Parity.None, 8);
            _port.Open();
        }

        #endregion

        #region Exposed Methods

        public void SendString(string value)
        {
            _port.Write(value.ToCharArray(), 0, value.Length);
        }

        public void Dispose()
        {
            if (_port.IsOpen)
            {
                SendString("0000        ");
                _port.Close();
            }
            _port.Dispose();
        }

        #endregion
    }
}