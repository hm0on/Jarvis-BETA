using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Net.Http;
using WindowsInput;
using System.Reflection;
using System.Windows.Media;
using log4net;
using Jarvis.Project.Settings.ConfigurationManager;
using Jarvis.Project.Services.VoskSpeechRecognition;
using Jarvis.Project.Services.HttpClientFolder;
using Jarvis.Project.Settings.PathManager;
using Jarvis.Project.Settings.AntiPiracy;
using Jarvis.Project.Settings.VoiceAssistant;
using Jarvis.Project.Services.WindowUtilities;

namespace Jarvis
{
    public partial class MainWindow : Window
    {
        private readonly VoskSpeechRecognition voskRecognizer;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static bool ON_vcom = true;

        private readonly Dictionary<string, string> exePaths = new Dictionary<string, string>();
        private readonly ExePathManagerClass pathManager = new ExePathManagerClass();

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                voskRecognizer = new VoskSpeechRecognition(Dispatcher);

                ContentRendered += MainWindow_Loaded;

                Thread.Sleep(1000);

                log4net.Config.XmlConfigurator.Configure();
                log.Info("[SYSTEM]: All assemblies have been loaded");


                SettingsManagerClass settingsManager = new SettingsManagerClass();
                string city = settingsManager.Load();

                log.Info("[WEATHER Parser]: User entered city: " + city);

                _ = WheaterClass.GetWeatherAsync(city);

                
            }
            catch (Exception ex)
            {
                log.Error($"[SYSTEM]: An error occurred in Jarvis when loading assemblies: {ex.Message}");
            }

            exePaths = ExePathManagerClass.LoadPathsFromJson();

            if (exePaths.Count == 0)
            {
                pathManager.UpdateExePaths(exePaths);
            }
        }

        private static void PreloadAccessibilityAssembly()
        {
            try
            {
                string AssemblyDllPath = Properties.Settings.Default.AssemblyDllPath;
                Assembly.LoadFrom(AssemblyDllPath);
            }
            catch
            {
                // Ignore load errors
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            voskRecognizer?.Dispose();
        }

        public void CloseApplication(string processName)
        {
            try
            {
                WindowManagerClass.CloseWindow(processName);
            }
            catch (Exception ex)
            {
                log.Error($"[SYSTEM]: Error when closing the process: {processName}. Error: {ex.Message}");
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
                log.Error($"[SYSTEM]: Error when minimizing the process: {processName}. Error: {ex.Message}");
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
                log.Error($"[SYSTEM]: Error when restoring the process: {processName}. Error: {ex.Message}");
            }
        }

        private async void SaveCity_OnClick(object sender, RoutedEventArgs e)
        {
            string city = CityBox.Text.Trim();

            if (string.IsNullOrEmpty(city))
            {
                SetErrorLabel("Введите город!", "#FF0000");
                return;
            }

            try
            {
                await WheaterClass.GetWeatherAsync(city);

                new Project.Settings.ConfigurationManager.SettingsManagerClass(city).Save();
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

        private void ButtonSettings_OnClick(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Hidden;
            SettingsGrid.Visibility = Visibility.Visible;
            ChangeGridButton(SettingsGridButton);
        }

        private async void MainWindow_Loaded(object sender, EventArgs e)
        {
            bool AntiPiracyStatus = Properties.Settings.Default.AntiPiracyStatus;
            if (AntiPiracyClass.IsFirstRun()) 
            {
                if (AntiPiracyStatus == true)
                {
                    await AntiPiracyClass.GetHwid_MBox_Y();
                    log.Info("[ANTI PIRACY]: First run check: YES");
                    log.Info("[ANTI PIRACY]: Mode: ON");
                }
                else
                {
                    log.Info("[ANTI PIRACY]: First run check: YES");
                    log.Warn("[ANTI PIRACY]: Mode: OFF");
                }
            }
            else
            {
                if (AntiPiracyStatus == true)
                {
                    await AntiPiracyClass.GetHwid_MBox_N();
                    log.Info("[ANTI PIRACY]: First run check: NO");
                    log.Info("[ANTI PIRACY]: Mode: ON");
                }
                else
                {
                    log.Info("[ANTI PIRACY]: First run check: YES");
                    log.Warn("[ANTI PIRACY]: Mode: OFF");
                }
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            log.Info("[SYSTEM]: Program shutdown: close button is pressed");
            Application.Current.Shutdown();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            log.Info("[SYSTEM]: Minimize the program: minimize button is pressed");
            this.WindowState = WindowState.Minimized;
        }

        private void Header_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => this.DragMove();

        private void Button_ON_Click(object sender, RoutedEventArgs e)
        {
            ON_vcom = !ON_vcom;
        }

        public void UpdateExePathsManually()
        {
            pathManager.UpdateExePaths(exePaths);
        }
    }
}