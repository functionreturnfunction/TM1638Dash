using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;
using StructureMap.Pipeline;

namespace TM1638Dash.Tests
{
    [TestClass]
    public class UsbDeviceHelperTest
    {
        #region Constants

        public const string PORT_NAME = "TestPort";

        #endregion

        #region Private Members

        private Mock<IContainer> _container;
        private Mock<IUsbDevice> _device;
        private UsbDeviceHelper _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Mock<IContainer>();
            _container.Setup(
                x =>
                    x.GetInstance<IUsbDevice>(It.Is<ExplicitArguments>(e => e.GetArg("portName").ToString() == PORT_NAME)))
                .Returns((_device = new Mock<IUsbDevice>(MockBehavior.Strict)).Object);
            _target = new UsbDeviceHelper(_container.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            
        }

        #endregion

        #region Helper Methods

        private void StartTarget()
        {
            _device.Setup(x => x.SendString(UsbDeviceHelper.DeviceMessages.READY));

            _target.Start(PORT_NAME);
        }

        private string GetLEDString(int idx)
        {
            return UsbDeviceHelper.LEFT_LEDS[idx].ToString("X").PadLeft(2, '0');
        }

        private void DisplayAndVerify(int maxRpms, int rpms, int gear, int kmh, string expected)
        {
            DisplayAndVerify(LEDStyle.Left, maxRpms, rpms, gear, kmh, expected);
        }

        private void DisplayAndVerify(LEDStyle style, int maxRpms, int rpms, int gear, int kmh, string expected)
        {
            _device.Setup(x => x.SendString(expected));

            _target.DisplayStats(new AssettoCorsaHelper.CurrentStats {
                Style = style,
                MaxRPM = maxRpms,
                RPM = rpms,
                Gear = gear,
                Speed = kmh
            });

        }

        #endregion

        [TestMethod]
        public void TestDisplayStatsSendsCorrectLightString()
        {
            StartTarget();

            DisplayAndVerify(100, 74, 0, 0, $"{GetLEDString(0)}00r      0");

            DisplayAndVerify(100, 75, 0, 0, $"{GetLEDString(1)}00r      0");
            DisplayAndVerify(100, 77, 0, 0, $"{GetLEDString(1)}00r      0");

            DisplayAndVerify(100, 78, 0, 0, $"{GetLEDString(2)}00r      0");
            DisplayAndVerify(100, 80, 0, 0, $"{GetLEDString(2)}00r      0");

            DisplayAndVerify(100, 81, 0, 0, $"{GetLEDString(3)}00r      0");
            DisplayAndVerify(100, 83, 0, 0, $"{GetLEDString(3)}00r      0");

            DisplayAndVerify(100, 84, 0, 0, $"{GetLEDString(4)}00r      0");
            DisplayAndVerify(100, 86, 0, 0, $"{GetLEDString(4)}00r      0");

            DisplayAndVerify(100, 87, 0, 0, $"{GetLEDString(5)}00r      0");
            DisplayAndVerify(100, 89, 0, 0, $"{GetLEDString(5)}00r      0");

            DisplayAndVerify(100, 90, 0, 0, $"{GetLEDString(6)}00r      0");
            DisplayAndVerify(100, 92, 0, 0, $"{GetLEDString(6)}00r      0");

            DisplayAndVerify(100, 93, 0, 0, $"{GetLEDString(7)}00r      0");
            DisplayAndVerify(100, 94, 0, 0, $"{GetLEDString(7)}00r      0");

            DisplayAndVerify(100, 95, 0, 0, $"{GetLEDString(8)}00r      0");
            DisplayAndVerify(100, 100, 0, 0, $"{GetLEDString(8)}00r      0");

            _device.VerifyAll();
        }

        [TestMethod]
        public void TestDisplayStatsSendsTheGearString()
        {
            StartTarget();

            // gear 0 is reverse
            DisplayAndVerify(1, 0, 0, 0, "0000r      0");

            // gear 1 is neutral
            DisplayAndVerify(1, 0, 0, 0, "0000n      0");

            // rest of the gears
            for (var i = 2; i < 10; ++i)
            {
                DisplayAndVerify(1, 0, 0, 0, $"0000{i - 1}      0");
            }
        }

        [TestMethod]
        public void TestDisplayStatsThrowsExceptionWhenNotYetStarted()
        {
            var thrown = false;

            try
            {
                _target.DisplayStats(new AssettoCorsaHelper.CurrentStats { Style = LEDStyle.Left, MaxRPM = 1, RPM = 0, Gear = 0, Speed = 0 });
            }
            catch (InvalidOperationException e) when (e.Message == UsbDeviceHelper.ExceptionMessages.NOT_YET_STARTED)
            {
                thrown = true;
            }

            Assert.IsTrue(thrown);
        }
    }
}
