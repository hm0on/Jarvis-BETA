using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Jarvis.Project.Services.HttpClientFolder;
using Jarvis.Project.Services.VoskSpeechRecognition;
using Jarvis.Project.Services.WindowUtilities;
using Jarvis.ProjectJarvis.Settings.AntiPiracy;
using Jarvis.ProjectJarvis.Settings.ConfigurationManager;
using Jarvis.ProjectJarvis.Settings.PathManager;
using Jarvis.ProjectJarvis.Settings.VoiceAssistant;
using Jarvis.Properties;
using log4net;
using log4net.Config;

namespace Jarvis;

public partial class MainWindow
{
    private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

    private readonly ExePathManagerClass _pathManager = new();
    private readonly VoskSpeechRecognition _voskRecognizer;

    public MainWindow()
    {
        try
        {
            InitializeComponent();

            _voskRecognizer = new VoskSpeechRecognition(Dispatcher);

            ContentRendered += MainWindow_Loaded;

            Thread.Sleep(1000);

            XmlConfigurator.Configure();
            Log.Info("[SYSTEM]: All assemblies have been loaded");


            var settingsManager = new SettingsManagerClass();
            var city = settingsManager.Load();

            Log.Info("[WEATHER Parser]: User entered city: " + city);

            _ = WheaterClass.GetWeatherAsync(city);
        }
        catch (Exception ex)
        {
            Log.Error($"[SYSTEM]: An error occurred in Jarvis when loading assemblies: {ex.Message}");
        }

        var exePaths = ExePathManagerClass.LoadPathsFromJson();

        if (exePaths.Count == 0) _pathManager.UpdateExePaths(exePaths);
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _voskRecognizer?.Dispose();
    }

    public void CloseApplication(string processName)
    {
        try
        {
            WindowManagerClass.CloseWindow(processName);
        }
        catch (Exception ex)
        {
            Log.Error($"[SYSTEM]: Error when closing the process: {processName}. Error: {ex.Message}");
        }
    }

    public void MinimizeWindow(string processName)
    {
        try
        {
            WindowManagerClass.MinimizeWindow(processName);
            VoiceJarvisClass.JarvisVoiceYes();
        }
        catch (Exception ex)
        {
            Log.Error($"[SYSTEM]: Error when minimizing the process: {processName}. Error: {ex.Message}");
        }
    }

    public void RestoreWindow(string processName)
    {
        try
        {
            WindowManagerClass.RestoreWindow(processName);
            VoiceJarvisClass.JarvisVoiceYes();
        }
        catch (Exception ex)
        {
            Log.Error($"[SYSTEM]: Error when restoring the process: {processName}. Error: {ex.Message}");
        }
    }

    private async void SaveCity_OnClick(object sender, RoutedEventArgs e)
    {
        var city = CityBox.Text.Trim();

        if (string.IsNullOrEmpty(city))
        {
            SetErrorLabel("Введите город!", "#FF0000");
            return;
        }

        try
        {
            await WheaterClass.GetWeatherAsync(city);

            new SettingsManagerClass(city).Save();
            SetErrorLabel("Город успешно сохранён!", "#7CFC00");
            VoiceJarvisClass.JarvisVoiceYes();
        }
        catch (HttpRequestException)
        {
            SetErrorLabel("Город не найден!", "#FF0000");
        }
        catch (Exception ex)
        {
            SetErrorLabel("Ошибка: " + ex.Message, "#FF0000");
        }
    }

    private void SetErrorLabel(string message, string color)
    {
        ErrorLabel.Foreground = new BrushConverter().ConvertFromString(color) as SolidColorBrush;
        ErrorLabel.Content = message;
    }

    private void ChangeGridButton(Button button)
    {
        MainGridButton.Background = new BrushConverter().ConvertFromString("#141414") as SolidColorBrush;
        SettingsGridButton.Background = new BrushConverter().ConvertFromString("#141414") as SolidColorBrush;
        button.Background = new BrushConverter().ConvertFromString("#4466FF") as SolidColorBrush;
    }

    private void ButtonMain_OnClick(object sender, RoutedEventArgs e)
    {
        MainGrid.Visibility = Visibility.Visible;
        SettingsGrid.Visibility = Visibility.Hidden;
        ChangeGridButton(MainGridButton);
    }

    private async void MainWindow_Loaded(object sender, EventArgs e)
    {
        var antiPiracyStatus = Settings.Default.AntiPiracyStatus;
        if (AntiPiracyClass.IsFirstRun())
        {
            if (antiPiracyStatus)
            {
                await AntiPiracyClass.GetHwid_MBox_Y();
                Log.Info("[ANTI PIRACY]: First run check: YES");
                Log.Info("[ANTI PIRACY]: Mode: ON");
            }
            else
            {
                Log.Info("[ANTI PIRACY]: First run check: YES");
                Log.Warn("[ANTI PIRACY]: Mode: OFF");
            }
        }
        else
        {
            if (antiPiracyStatus)
            {
                await AntiPiracyClass.GetHwid_MBox_N();
                Log.Info("[ANTI PIRACY]: First run check: NO");
                Log.Info("[ANTI PIRACY]: Mode: ON");
            }
            else
            {
                Log.Info("[ANTI PIRACY]: First run check: YES");
                Log.Warn("[ANTI PIRACY]: Mode: OFF");
            }
        }
    }

    private void ButtonClose_Click(object sender, RoutedEventArgs e)
    {
        Log.Info("[SYSTEM]: Program shutdown: close button is pressed");
        Application.Current.Shutdown();
    }

    private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
    {
        Log.Info("[SYSTEM]: Minimize the program: minimize button is pressed");
        WindowState = WindowState.Minimized;
    }

    private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
}