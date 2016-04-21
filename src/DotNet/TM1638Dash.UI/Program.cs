using System;
using System.Windows.Forms;

namespace TM1638Dash
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var container = IocHelper.GetContainer(e => {
                e.For<MainForm>().Use<MainForm>().Singleton();
                e.For<ILog>().Use<MainFormLogger>();
            });
            Application.Run(container.GetInstance<MainForm>());
        }
    }
}
