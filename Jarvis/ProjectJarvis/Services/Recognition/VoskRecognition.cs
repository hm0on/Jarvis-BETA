using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using log4net;
using NAudio.Wave;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Jarvis.ProjectJarvis.CommandsFolder;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace Jarvis.Project.Services.VoskSpeechRecognition
{
    public class VoskSpeechRecognition : IDisposable
    {
        private readonly Dispatcher _dispatcher;
        private VoskWrapper vosk;
        private WaveInEvent waveIn;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public VoskSpeechRecognition(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            InitializeVosk();
        }

        public void InitializeVosk()
        {
            // Получаем текущую директорию исполняемого файла
            var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Поднимаемся на два уровня вверх от текущей директории
            var projectDir = Directory.GetParent(appDir).Parent.Parent.FullName;

            // Относительный путь к модели
            string relativePath = Properties.Settings.Default.relativePathVoskModel;

            // Комбинируем проектную директорию с относительным путем
            var fullPath = Path.Combine(projectDir, relativePath);

            vosk = new VoskWrapper(fullPath, 16000.0f);

            waveIn = new WaveInEvent();
            waveIn.WaveFormat = new WaveFormat(16000, 1);
            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.StartRecording();

            log.Info("[VOSK]: Successful initialization. The VOSK model has been successfully initialized.");
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            string result = vosk.Recognize(e.Buffer);
            if (!string.IsNullOrEmpty(result))
            {
                var json = JObject.Parse(result);
                var text = json["text"].ToString();

                if (!string.IsNullOrEmpty(text))
                {
                    var words = text.Split(' ');
                    if (words.Length > 1)
                    {
                        var command = words[0];
                        var service = words[1];

                        _dispatcher.Invoke(() =>
                        {
                            log.Info($"[ACTION RECOGNIZED]: command: '{command}', service: '{service}'");
                            OpenService(service, command);
                        });
                    }
                }
            }
        }

        private Dictionary<Func<string, bool>, Action> VoskCommandProcess(string service, string command)
        {
            return new Dictionary<Func<string, bool>, Action>
            {
                { 
                    cmd => ActionsListClass.openCommands.Contains(command), () =>
                    {
                        bool programOpened = false;
                        var exePaths = Jarvis.Project.Settings.PathManager.ExePathManagerClass.LoadPathsFromJson();
                        foreach (var entry in ExeCommandsClass.exeNameProgramms)
                        {
                            if (entry.Key.Contains(service))
                            {
                                string exeName = entry.Value;
                                if (exePaths.TryGetValue(exeName, out string exePath))
                                {
                                    Process.Start(new ProcessStartInfo(exePath) { UseShellExecute = true });
                                    log.Info($"[OPENING THE PROGRAM]: opening the program in the path: '{exePath}'.");
                                    programOpened = true;
                                    break;
                                }
                                else
                                {
                                    log.Error($"[OPENING THE PROGRAM]: path '{exeName}' not found.");
                                }
                            }
                        }

                        // Если программа не была открыта, попытаться открыть сайт
                        if (!programOpened)
                        {
                            foreach (var entry in UrlCommandsClass.urlCommands)
                            {
                                if (entry.Key.Contains(service))
                                {
                                    Process.Start(new ProcessStartInfo(entry.Value) { UseShellExecute = true });
                                    log.Info($"[OPEN THE SITE]: opening the site by this URL: '{entry.Value}'.");
                                    return;
                                }
                            }

                            log.Error($"[OPEN THE SITE / PROGRAM]: Service or program '{service}' not found.");
                        }

                        Settings.VoiceAssistant.VoiceJarvisClass.JarvisVoiceYes();
                    }
                },
                { 
                    cmd => ActionsListClass.closeCommands.Contains(command), () =>
                    {
                        foreach (var entry in ExeCommandsClass.exeNameProgramms)
                        {
                            if (entry.Key.Contains(service))
                            {
                                Process[] processes = Process.GetProcessesByName(entry.Value.Replace(".exe",""));
                                foreach(Process p in processes)
                                {
                                   p.Kill();
                                }
                            }
                        }
                        Settings.VoiceAssistant.VoiceJarvisClass.JarvisVoiceYes();
                    }
                }
            };
        }


        private void OpenService(string service, string command)
        {
            var commandsHandlers = VoskCommandProcess(service, command);

            foreach (var handler in commandsHandlers)
            {
                if (handler.Key(command))
                {
                    handler.Value();
                    break;
                }
            }
        }

        public void Dispose()
        {
            waveIn.DataAvailable -= WaveIn_DataAvailable;
            waveIn?.StopRecording();
            waveIn?.Dispose();
            vosk?.Dispose();
        }
    }
}
