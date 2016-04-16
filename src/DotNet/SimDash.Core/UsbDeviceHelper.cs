using System;
using System.Collections.Generic;
using StructureMap;
using StructureMap.Pipeline;

namespace SimDash
{
    public class UsbDeviceHelper : IUsbDeviceHelper
    {
        #region Constants

        public static readonly int[] LEDS = {1, 3, 7, 15, 31, 8223, 24607, 57375};

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

        private string BuildCommandString(int maxRpms, int rpms, int gear, float speedKmh)
        {
            return DetermineLights(maxRpms, rpms) + "00" + FixGear(gear) + FixSpeed(speedKmh);
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
            return 1 + (percentage - 61)/(90 - 61)*7;
        }

        private string DetermineLights(int maxRpms, int rpms)
        {
            if (rpms == 0 || maxRpms == 0)
            {
                return "00";
            }

            var percentage = rpms * 100.0 / maxRpms;

            if (percentage < 60)
            {
                return "00";
            }

            var scaled = ScaleRPMs((int)Math.Round(percentage));

            return LEDS[scaled - 1].ToString("X").PadLeft(2, '0');
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

        public void DisplayStats(int maxRpms, int rpms, int gear, float speedKmh)
        {
            if (!Started)
            {
                throw new InvalidOperationException(ExceptionMessages.NOT_YET_STARTED);
            }

            _device.SendString(BuildCommandString(maxRpms, rpms, gear, speedKmh));
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
        void DisplayStats(int maxRpms, int rpms, int gear, float speedKmh);

        #endregion
    }
}