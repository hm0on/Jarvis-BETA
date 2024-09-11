using System;
using System.Windows;
using Jarvis.ProjectJarvis.Services.Authentificate;
using log4net;
using log4net.Config;
using Microsoft.Extensions.DependencyInjection;

namespace Jarvis;

public partial class App
{
    private static readonly ILog Log = LogManager.GetLogger(typeof(App));
    public IServiceProvider ServiceProvider { get; private set; }
    protected override void OnStartup(StartupEventArgs e)
    {
        XmlConfigurator.Configure();
        Log.Info("\n\n ============= Application Started ============= ");
        base.OnStartup(e);
        try
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IAuthentificateService, AuthentificateService>();
    }
}