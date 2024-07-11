using System.Globalization;
using System.Linq;
using Jarvis.ProjectJarvis;
using log4net;
using System.Diagnostics;
using System.Windows.Threading;
using Jarvis.Project.Settings;
using System.Speech.Recognition;
using Jarvis.Assets.SpeechResources;
using Jarvis.ProjectJarvis.CommandsFolder;

namespace Jarvis.Project.Services.SpeechRecognition
{
    public class SpeechRecognitionClass
    {
        private readonly Dispatcher _dispatcher;
        private SpeechRecognitionEngine recognizer;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SpeechRecognitionClass(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            InitializeSpeechRecognition();
        }

        public void InitializeSpeechRecognition()
        {
            recognizer = new SpeechRecognitionEngine(new CultureInfo("ru-RU"));

            // ДЕЙСТВИЯ
            Choices commands = new Choices();
            commands.Add(ActionsListClass.openCommands.ToArray());
            commands.Add(ActionsListClass.closeCommands.ToArray());
            commands.Add(ActionsListClass.minimizeCommands.ToArray());
            commands.Add(ActionsListClass.upCommands.ToArray());
            commands.Add(ActionsListClass.maximizeCommands.ToArray());
            commands.Add(ActionsListClass.standartsizeCommands.ToArray());
            commands.Add(ActionsListClass.clearCommands.ToArray());
            commands.Add(ActionsListClass.shutdownCommands.ToArray());
            commands.Add(ActionsListClass.restartCommands.ToArray());
            commands.Add(SystemCommandsListClass.hibernateCommands.ToArray());  // ОБРАТИ ВНИМАНИЕ!!!! ТУТ ДРУГОЙ КЛАСС!!
            commands.Add(ActionsListClass.changeCommands.ToArray());
            commands.Add(ActionsListClass.volumecontrolCommands.ToArray());
            commands.Add(ActionsListClass.makeCommands.ToArray());
            commands.Add(ActionsListClass.restoreCommands.ToArray());
            commands.Add(ActionsListClass.otherCommands.ToArray());


            // САЙТЫ
            Choices sites = new Choices();
            sites.Add(SiteListClass.youtubeCommands.ToArray());
            sites.Add(ProgramListClass.telegramCommands.ToArray());
            sites.Add(SiteListClass.vkontakteCommands.ToArray());
            sites.Add(SiteListClass.netflixCommands.ToArray());
            sites.Add(SiteListClass.twichCommands.ToArray());
            sites.Add(ProgramListClass.yandexCommands.ToArray());
            sites.Add(ProgramListClass.googleCommands.ToArray());
            sites.Add(ProgramListClass.microsoftedgeCommands.ToArray());
            sites.Add(ProgramListClass.yandexmusicCommands.ToArray());
            sites.Add(SiteListClass.yandexplayCommands.ToArray());
            sites.Add(SiteListClass.yandexmapCommands.ToArray());
            sites.Add(SiteListClass.yandexweatherCommands.ToArray());
            sites.Add(SiteListClass.yandexmailCommands.ToArray());
            sites.Add(SiteListClass.googlemailCommands.ToArray());
            sites.Add(SiteListClass.vkmailCommands.ToArray());
            sites.Add(ProgramListClass.steamCommands.ToArray());
            sites.Add(ProgramListClass.epicgamesCommands.ToArray());
            sites.Add(ProgramListClass.whatsappCommands.ToArray());
            sites.Add(ProgramListClass.discordCommands.ToArray());
            sites.Add(ProgramListClass.winfilesexplorerCommands.ToArray());
            sites.Add(ProgramListClass.winsettingsCommands.ToArray());
            sites.Add(ProgramListClass.microsoftstoreCommands.ToArray());
            sites.Add(ProgramListClass.spotifyCommands.ToArray());
            sites.Add(ProgramListClass.binCommands.ToArray());
            sites.Add(SystemCommandsListClass.workCommands.ToArray());  // ОБРАЩАЙ ВНИМАНИЕ НА КЛАССЫ!!!
            sites.Add(SystemCommandsListClass.languageCommands.ToArray());
            sites.Add(ActionsListClass.volumemaxCommands.ToArray());
            sites.Add(ActionsListClass.volumeaddCommands.ToArray());
            sites.Add(ActionsListClass.volumemediumCommands.ToArray());
            sites.Add(ActionsListClass.volumelowCommands.ToArray());
            sites.Add(ActionsListClass.volumedelCommands.ToArray());
            sites.Add(SystemCommandsListClass.tabCommands.ToArray());
            sites.Add(SystemCommandsListClass.historyCommands.ToArray());
            sites.Add(SystemCommandsListClass.windowCommands.ToArray());
            sites.Add(SystemCommandsListClass.downloadsCommands.ToArray());



            // ГРАММАТИКА
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(commands);
            grammarBuilder.Append(sites);

            Grammar grammar = new Grammar(grammarBuilder);

            recognizer.LoadGrammar(grammar);
            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
            log.Info("Recognizer initialized: COMPLETED.");
        }


        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string command = e.Result.Words[0].Text;
            string service = e.Result.Words[1].Text;

            _dispatcher.Invoke(() =>
            {
                log.Info($"NEW ACTION: command: '{command}', service: '{service}'");
                openService(service, command);
            });
        }

        private void openService(string service, string command)
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

    }

}
