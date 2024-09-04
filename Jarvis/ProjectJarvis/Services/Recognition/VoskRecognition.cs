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

        public enum RecognitionMode
        {
            AlwaysListening,  // Режим 1: всегда слушает
            JarvisListening,  // Режим 2: слушает после слова "джарвис"
            Disabled          // Режим 3: не слушает вообще
        }

        private RecognitionMode currentMode;
        private bool isListeningForCommand = false;
        private DateTime lastCommandTime;
        private readonly TimeSpan commandTimeout = TimeSpan.FromSeconds(Properties.Settings.Default.RecognitionWaitTime);
        private readonly TimeSpan postCommandListeningTime = TimeSpan.FromSeconds(Properties.Settings.Default.RecognitionPostCommandWaitTime); // Время ожидания после выполнения команды

        public VoskSpeechRecognition(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            InitializeVosk();
            currentMode = RecognitionMode.JarvisListening;  // Начальный режим
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
            if (currentMode == RecognitionMode.Disabled)
            {
                return;
            }

            string result = vosk.Recognize(e.Buffer);
            if (!string.IsNullOrEmpty(result))
            {
                var json = JObject.Parse(result);
                var text = json["text"].ToString().ToLower();

                if (currentMode == RecognitionMode.JarvisListening && !isListeningForCommand)
                {
                    if (text.Contains("джарвис"))
                    {
                        isListeningForCommand = true;
                        lastCommandTime = DateTime.Now;
                        log.Info("[RECOGNITION MODE]: Activated listening mode.");
                        Settings.VoiceAssistant.VoiceJarvisClass.JarvisVoiceComplete();
                        return;
                    }
                }
                else if (isListeningForCommand || currentMode == RecognitionMode.AlwaysListening)
                {
                    if (isListeningForCommand && (DateTime.Now - lastCommandTime) > commandTimeout)
                    {
                        isListeningForCommand = false;
                        log.Info("[RECOGNITION MODE]: Command timeout, disabling listening mode.");
                        return;
                    }

                    if (!string.IsNullOrEmpty(text))
                    {
                        lastCommandTime = DateTime.Now;
                        ProcessCommand(text);

                        if (currentMode == RecognitionMode.JarvisListening)
                        {
                            // Ждём дополнительные время после выполнения команды
                            lastCommandTime = DateTime.Now.Add(postCommandListeningTime);
                            log.Info($"[RECOGNITION MODE]: Waiting for {postCommandListeningTime.TotalSeconds} seconds after command execution.");
                        }
                    }
                }
            }
        }

        private void ProcessCommand(string text)
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

        public void SetRecognitionMode(RecognitionMode mode)
        {
            currentMode = mode;
            isListeningForCommand = false;
            log.Info($"[RECOGNITION MODE]: Recognition mode switched to {mode}");
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