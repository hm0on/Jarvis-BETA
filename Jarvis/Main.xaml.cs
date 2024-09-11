using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Net.Http;
using System.Reflection;
using System.Windows.Media;
using log4net;
using Jarvis.Project.Services.VoskSpeechRecognition;
using Jarvis.Project.Services.HttpClientFolder;
using Jarvis.Project.Services.WindowUtilities;
using Jarvis.ProjectJarvis.Settings.AntiPiracy;
using Jarvis.ProjectJarvis.Settings.ConfigurationManager;
using Jarvis.ProjectJarvis.Settings.PathManager;
using Jarvis.ProjectJarvis.Settings.VoiceAssistant;

namespace Jarvis
{
    public partial class MainWindow
    {
        private readonly VoskSpeechRecognition _voskRecognizer;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static bool _onVcom = true;

        private readonly Dictionary<string, string> _exePaths;
        private readonly ExePathManagerClass _pathManager = new();

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                _voskRecognizer = new VoskSpeechRecognition(Dispatcher);

                ContentRendered += MainWindow_Loaded;

                Thread.Sleep(1000);

                log4net.Config.XmlConfigurator.Configure();
                Log.Info("[SYSTEM]: All assemblies have been loaded");


                SettingsManagerClass settingsManager = new SettingsManagerClass();
                string city = settingsManager.Load();

                Log.Info("[WEATHER Parser]: User entered city: " + city);

                _ = WheaterClass.GetWeatherAsync(city);

                
            }
            catch (Exception ex)
            {
                Log.Error($"[SYSTEM]: An error occurred in Jarvis when loading assemblies: {ex.Message}");
            }

            _exePaths = ExePathManagerClass.LoadPathsFromJson();

            if (_exePaths.Count == 0)
            {
                _pathManager.UpdateExePaths(_exePaths);
            }
        }

        private static void PreloadAccessibilityAssembly()
        {
            try
            {
                string assemblyDllPath = Properties.Settings.Default.AssemblyDllPath;
                Assembly.LoadFrom(assemblyDllPath);
            }
            catch
            {
                // Ignore load errors
            }
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
            string city = CityBox.Text.Trim();

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
            MainFrame.Navigate(new Uri(@"ProjectJarvis\Views\Pages\Primary\Basic.xaml", UriKind.Relative));
            // MainGrid.Visibility = Visibility.Visible;
            // SettingsGrid.Visibility = Visibility.Hidden;
            // ChangeGridButton(MainGridButton);
        }

        private void ButtonCommandsList_OnClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri(@"ProjectJarvis\Views\Pages\SidePanel\CommandsList\CommandsListPage.xaml", UriKind.Relative));
        }

        private void ButtonInformation_OnClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri(@"ProjectJarvis\Views\Pages\SidePanel\Information\InformationPage.xaml", UriKind.Relative));
        }

        private void ButtonSettings_OnClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri(@"ProjectJarvis\Views\Pages\SidePanel\Settings\SettingsPage.xaml", UriKind.Relative));
            // MainGrid.Visibility = Visibility.Hidden;
            // SettingsGrid.Visibility = Visibility.Visible;
            // ChangeGridButton(SettingsGridButton);
        }

        private void ButtonProfile_OnClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri(@"ProjectJarvis\Views\Pages\SidePanel\Profile\ProfilePage.xaml", UriKind.Relative));
            // MainGrid.Visibility = Visibility.Hidden;
            // ProfileGrid.Visibility = Visibility.Visible;
            // ChangeGridButton(ProfileGridButton);
        }

        private async void MainWindow_Loaded(object sender, EventArgs e)
        {
            bool AntiPiracyStatus = Properties.Settings.Default.AntiPiracyStatus;
            if (AntiPiracyClass.IsFirstRun()) 
            {
                if (AntiPiracyStatus == true)
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
                if (AntiPiracyStatus == true)
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
            this.WindowState = WindowState.Minimized;
        }

        private void Header_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => this.DragMove();

        private void Button_ON_Click(object sender, RoutedEventArgs e)
        {
            _onVcom = !_onVcom;
        }

        public void UpdateExePathsManually()
        {
            _pathManager.UpdateExePaths(_exePaths);
        }
    }
}