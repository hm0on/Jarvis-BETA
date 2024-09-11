using System.Windows;
using log4net;
using log4net.Config;

namespace Jarvis;

public partial class App
{
    private static readonly ILog Log = LogManager.GetLogger(typeof(App));

    protected override void OnStartup(StartupEventArgs e)
    {
        XmlConfigurator.Configure();
        Log.Info("\n\n ============= Application Started ============= ");
        base.OnStartup(e);
    }
}