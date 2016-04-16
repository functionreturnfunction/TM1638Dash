using StructureMap;

namespace SimDash
{
    public static class IocHelper
    {
        #region Private Methods

        private static void ConfigureContainer(ConfigurationExpression c)
        {
            c.For<IAssettoCorsaHelper>().Use<AssettoCorsaHelper>();
            c.For<IUsbDeviceHelper>().Singleton().Use<UsbDeviceHelper>();
            c.For<IUsbDevice>().Use<UsbDevice>();
        }

        #endregion

        #region Exposed Methods

        public static IContainer GetContainer()
        {
            return new Container(ConfigureContainer);
        }

        #endregion
    }
}