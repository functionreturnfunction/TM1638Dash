using System;
using System.Windows.Forms;
using StructureMap;

namespace SimDash
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = IocHelper.GetContainer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.GetInstance<MainForm>());
        }
    }

    public static class IocHelper
    {
        public static IContainer GetContainer()
        {
            return new Container(ConfigureContainer);
        }

        private static void ConfigureContainer(ConfigurationExpression c)
        {
            c.For<IAssettoCorsaHelper>().Use<AssettoCorsaHelper>();
            c.For<IUsbDeviceHelper>().Singleton().Use<UsbDeviceHelper>();
            c.For<IUsbDevice>().Use<UsbDevice>();
        }
    }
}
