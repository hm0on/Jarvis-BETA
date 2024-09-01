using System.Collections.Generic;

namespace Jarvis.ProjectJarvis.CommandsFolder
{
    public static class ExeCommandsClass
    {
        public static readonly Dictionary<HashSet<string>, string> exeNameProgramms = new Dictionary<HashSet<string>, string>
        {
            { ProgramListClass.telegramCommands, "Telegram.exe" },
            { ProgramListClass.steamCommands, "steam.exe" },
            { ProgramListClass.epicgamesCommands, "EpicGamesLauncher.exe" },
            { ProgramListClass.discordCommands, "Update.exe" },
            { ProgramListClass.yandexCommands, "browser.exe" },
            { ProgramListClass.whatsappCommands, "WhatsApp.exe" },
            { ProgramListClass.googleCommands, "chrome.exe" },
            { ProgramListClass.spotifyCommands, "Spotify.exe" },
            { ProgramListClass.microsoftedgeCommands, "msedge.exe" },
            { ProgramListClass.yandexmusicCommands, "Яндекс Музыка.exe" }
        };
    }
}