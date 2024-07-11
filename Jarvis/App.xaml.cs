using System.Windows;
using log4net;

namespace Jarvis
{
    public partial class App : Application
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        protected override void OnStartup(StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Info("\n\n ============= Application Started ============= ");
            base.OnStartup(e);
        }
    }
}