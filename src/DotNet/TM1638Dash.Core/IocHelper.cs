using System;
using StructureMap;

namespace TM1638Dash
{
    public static class IocHelper
    {
        #region Private Methods

        private static Action<ConfigurationExpression> GetConfigurationFn(Action<ConfigurationExpression> configure)
        {
            return c => {
                c.For<IAssettoCorsaHelper>().Use<AssettoCorsaHelper>();
                c.For<IUsbDeviceHelper>().Singleton().Use<UsbDeviceHelper>();
                c.For<IUsbDevice>().Use<UsbDevice>();
                configure(c);
            };
        }

        #endregion

        #region Exposed Methods

        public static IContainer GetContainer(Action<ConfigurationExpression> configure = null)
        {
            configure = configure ?? (_ => { });

            return new Container(GetConfigurationFn(configure));
        }

        #endregion
    }
}