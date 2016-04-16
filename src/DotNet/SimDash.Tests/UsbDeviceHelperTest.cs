using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;
using StructureMap.Pipeline;

namespace SimDash.Tests
{
    [TestClass]
    public class UsbDeviceHelperTest
    {
        public const string PORT_NAME = "TestPort";
        private Mock<IContainer> _container;
        private Mock<IUsbDevice> _device;
        private UsbDeviceHelper _target;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Mock<IContainer>();
            _container.Setup(
                x =>
                    x.GetInstance<IUsbDevice>(It.Is<ExplicitArguments>(e => e.GetArg("portName").ToString() == PORT_NAME)))
                .Returns((_device = new Mock<IUsbDevice>()).Object);
            _target = new UsbDeviceHelper(_container.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            
        }

        private void StartTarget()
        {
            _target.Start(PORT_NAME);

            _device.Verify(x => x.SendString(UsbDeviceHelper.DeviceMessages.READY));
        }

        [TestMethod]
        public void TestDisplayStatsThrowsExceptionWhenNotYetStarted()
        {
            var thrown = false;

            try
            {
                _target.DisplayStats(1, 0, 0, 0);
            }
            catch (InvalidOperationException e) when (e.Message == UsbDeviceHelper.ExceptionMessages.NOT_YET_STARTED)
            {
                thrown = true;
            }

            Assert.IsTrue(thrown);
        }

        [TestMethod]
        public void TestDisplayStatsSendsTheGearString()
        {
            StartTarget();

            // gear 0 is reverse
            _target.DisplayStats(1, 0, 0, 0);

            _device.Verify(x => x.SendString("0000r      0"));

            // gear 1 is neutral
            _target.DisplayStats(1, 0, 1, 0);

            _device.Verify(x => x.SendString("0000n      0"));

            // rest of the gears
            for (var i = 2; i < 10; ++i)
            {
                _target.DisplayStats(1, 0, i, 0);

                _device.Verify(x => x.SendString($"0000{i - 1}      0"));
            }
        }
    }
}
