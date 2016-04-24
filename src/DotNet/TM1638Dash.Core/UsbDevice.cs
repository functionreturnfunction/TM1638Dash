using System;
using System.IO.Ports;

namespace TM1638Dash
{
    public class UsbDevice : IDisposable, IUsbDevice
    {
        #region Constants

        public const int BAUD = 19200,
            TX_LENGTH = 92;

        #endregion

        #region Private Members

        private readonly SerialPort _port;
        private readonly ILog _log;

        #endregion

        #region Properties

        public bool Connected => _port.IsOpen;

        #endregion

        #region Constructors

        public UsbDevice(string portName, ILog log)
        {
            _port = new SerialPort(portName, BAUD, Parity.None, 8);
            _log = log;
            _log.Info($"Opening serial connection to port '{portName}'");
            _port.Open();
        }

        #endregion

        #region Exposed Methods

        public void SendString(string value)
        {
            if (value.Length != TX_LENGTH)
            {
                throw new ArgumentException(
                    $"Cannot send string '{value}' because it's {(value.Length < TX_LENGTH ? "less" : "greater")} than the expected message size.");
            }

            _log.Info($"Writing command string '{value}'");
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

    public enum LEDStyle
    {
        Center, Left, Right
    }
}