using System;
using System.Collections.Generic;
using StructureMap;
using StructureMap.Pipeline;

namespace SimDash
{
    public class UsbDeviceHelper : IUsbDeviceHelper
    {
        #region Constants

        public static readonly int[] LEFT_LEDS =   {0,   1,   3,   7,  15,  31,  63, 127, 255};
        public static readonly int[] RIGHT_LEDS =  {0,  64, 192, 224, 240, 248, 252, 254, 255};
        public static readonly int[] CENTER_LEDS = {0, 129, 129, 195, 195, 195, 231, 231, 255};

        public static readonly Dictionary<LEDStyle, int[]> LED_STYLES = new Dictionary<LEDStyle, int[]> {
            {LEDStyle.Left, LEFT_LEDS},
            {LEDStyle.Right, RIGHT_LEDS},
            {LEDStyle.Center, CENTER_LEDS}
        };

        public struct RpmScale
        {
            public const int BEGIN = 75, END = 95;
        }

        public struct DeviceMessages
        {
            public const string READY = "0000ready   ";
        }

        public struct ExceptionMessages
        {
            public const string NOT_YET_STARTED = "Not yet started!",
                ALREADY_STARTED = "Already started!";
        }

        #endregion

        #region Private Members

        private readonly IContainer _container;
        private IUsbDevice _device;

        #endregion

        #region Properties

        public bool Started => _device != null;

        #endregion

        #region Constructors

        public UsbDeviceHelper(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Private Methods

        private string BuildCommandString(LEDStyle style, int maxRpms, int rpms, int gear, float speedKmh)
        {
            return DetermineLights(style, maxRpms, rpms) + "00" + FixGear(gear) + FixSpeed(speedKmh);
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

        private int ScaleRPMs(int percentage)
        {
            var newScale = (double)7/(RpmScale.END - RpmScale.BEGIN);
            var scale = (int)(1 + ((percentage - RpmScale.BEGIN)*newScale));

            return scale >= LEFT_LEDS.Length ? LEFT_LEDS.Length - 1 : scale;
        }

        private string DetermineLights(LEDStyle style, int maxRpms, int rpms)
        {
            if (rpms == 0 || maxRpms == 0)
            {
                return "00";
            }

            var percentage = rpms * 100.0 / maxRpms;

            if (percentage < RpmScale.BEGIN)
            {
                return "00";
            }

            var scaled = ScaleRPMs((int)percentage);
            var ledHex = LED_STYLES[style][scaled].ToString("X");

            return ledHex.PadLeft(2, '0');
        }

        #endregion

        #region Exposed Methods

        public void Start(string portName)
        {
            if (Started)
            {
                throw new InvalidOperationException(ExceptionMessages.ALREADY_STARTED);
            }

            _device =
                _container.GetInstance<IUsbDevice>(
                    new ExplicitArguments(new Dictionary<string, object> {{"portName", portName}}));
            _device.SendString(DeviceMessages.READY);
        }

        public void Stop()
        {
            if (!Started)
            {
                throw new InvalidOperationException(ExceptionMessages.NOT_YET_STARTED);
            }

            _device.Dispose();
            _device = null;
        }

        public void DisplayStats(LEDStyle style, int maxRpms, int rpms, int gear, float speedKmh)
        {
            if (!Started)
            {
                throw new InvalidOperationException(ExceptionMessages.NOT_YET_STARTED);
            }

            _device.SendString(BuildCommandString(style, maxRpms, rpms, gear, speedKmh));
        }

        #endregion
    }

    public interface IUsbDeviceHelper
    {
        #region Abstract Properties

        bool Started { get; }

        #endregion

        #region Abstract Methods

        void Start(string portName);
        void Stop();
        void DisplayStats(LEDStyle style, int maxRpms, int rpms, int gear, float speedKmh);

        #endregion
    }
}