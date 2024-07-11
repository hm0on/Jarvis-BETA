using Jarvis.Assets.SpeechResources;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarvis.ProjectJarvis.CommandsFolder
{
    public static class UrlCommandsClass
    {
        public static readonly Dictionary<HashSet<string>, string> urlCommands = new Dictionary<HashSet<string>, string>
        {
            { SiteListClass.youtubeCommands, "https://www.youtube.com/" },
            { SiteListClass.yandexmapCommands, "https://yandex.com/maps" },
            { SiteListClass.vkontakteCommands, "https://vk.com/" },
            { SiteListClass.netflixCommands, "https://www.netflix.com/" },
            { SiteListClass.twichCommands, "https://www.twitch.tv/" },
            { ProgramListClass.yandexCommands, "https://yandex.ru/" },
            { ProgramListClass.googleCommands, "https://www.google.ru/" },
            { ProgramListClass.telegramCommands, "https://web.telegram.org/k/" },
            { ProgramListClass.yandexmusicCommands, "https://music.yandex.ru/" },  // ЕСЛИ ЧТО ТУТ КЛАССЫ РАЗНЫЕ!!
            { SiteListClass.yandexplayCommands, "https://yandex.ru/games/" },
            { SiteListClass.yandexweatherCommands, "https://yandex.ru/pogoda/" },
            { SiteListClass.yandexmailCommands, "https://mail.yandex.ru/" },
            { SiteListClass.googlemailCommands, "https://mail.google.com/mail/" },
            { SiteListClass.vkmailCommands, "https://mail.ru/" },
            { ProgramListClass.steamCommands, "https://store.steampowered.com/" },
            { ProgramListClass.epicgamesCommands, "https://store.epicgames.com/" },
            { ProgramListClass.whatsappCommands, "https://web.whatsapp.com/" },
            { ProgramListClass.discordCommands, "https://discord.com/app" },
            { ProgramListClass.spotifyCommands, "https://open.spotify.com/" }
        };
    }
}
