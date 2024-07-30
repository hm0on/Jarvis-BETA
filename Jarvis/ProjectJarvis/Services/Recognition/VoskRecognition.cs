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
            var modelPath = "D:\\Prog\\vosk-model-small-ru-0.22";
            vosk = new VoskWrapper(modelPath, 16000.0f);

            waveIn = new WaveInEvent();
            waveIn.WaveFormat = new WaveFormat(16000, 1);
            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.StartRecording();

            log.Info("Vosk recognizer initialized: COMPLETED.");
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
                            log.Info($"NEW ACTION: command: '{command}', service: '{service}'");
                            OpenService(service, command);
                        });
                    }
                }
            }
        }

        private void OpenService(string service, string command)
        {
            bool programOpened = false;

            // Открытие программ
            var exePaths = Jarvis.Project.Settings.PathManager.ExePathManagerClass.LoadPathsFromJson();
            foreach (var entry in ExeCommandsClass.exeNameProgramms)
            {
                if (entry.Key.Contains(service))
                {
                    string exeName = entry.Value;
                    if (exePaths.TryGetValue(exeName, out string exePath))
                    {
                        Process.Start(new ProcessStartInfo(exePath) { UseShellExecute = true });
                        log.Info($"Opened program: '{exePath}'.");
                        programOpened = true;
                        break;
                    }
                    else
                    {
                        log.Warn($"Path for the program '{exeName}' not found.");
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
                        log.Info($"Open site: '{entry.Value}'.");
                        return;
                    }
                }

                log.Warn($"Service or program '{service}' not found.");
            }
        }

        public void Dispose()
        {
            waveIn?.StopRecording();
            waveIn?.Dispose();
            vosk?.Dispose();
        }
    }
}
