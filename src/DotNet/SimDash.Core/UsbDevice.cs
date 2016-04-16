using System;
using System.IO.Ports;

namespace SimDash
{
    public class UsbDevice : IDisposable, IUsbDevice
    {
        #region Constants

        public const int BAUD = 9600;

        #endregion

        #region Private Members

        private readonly SerialPort _port;

        #endregion

        #region Properties

        public bool Connected => _port.IsOpen;

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
            if (value.Length != 12)
            {
                throw new ArgumentException(
                    $"Cannot send string '{value}' because it's {(value.Length < 12 ? "less" : "greater")} than the expected message size.");
            }

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

    public interface IUsbDevice
    {
        #region Abstract Properties

        bool Connected { get; }

        #endregion

        #region Abstract Methods

        void Dispose();
        void SendString(string value);

        #endregion
    }
}