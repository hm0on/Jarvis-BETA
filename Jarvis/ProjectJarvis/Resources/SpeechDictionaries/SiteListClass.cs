using System;
using System.Collections.Generic;

namespace Jarvis.Assets.SpeechResources
{
    public static class SiteListClass
    {
        // СПИСКИ НАЗВАНИЯ САЙТОВ
        public static readonly HashSet<string> youtubeCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ютуб", "утуб", "youtube", "ютюб", "утюб", "ютьюб", "youtub", "utub", "ютуб", "ютюбэ", "йоутуб", "ютьюб", "ютьюбэ", "ютубэ", "йутуб"
        };

        public static readonly HashSet<string> vkontakteCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "вк", "вэка", "вконтакте", "вконтакт", "вэкашку", "вконтакти"
        };

        public static readonly HashSet<string> netflixCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "нетфликс", "нетликс", "нетфликса", "нетфликсы", "нетфликц", "netflix"
        };

        public static readonly HashSet<string> twichCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "twitch", "twitsh", "twich", "твич", "твичь", "твитч", "твичт", "твичэ", "твиш", "твишч"
        };

        public static readonly HashSet<string> yandexplayCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        { 
            "yandexplay", "yandexgame", "яндексигры", "яндексплей", "яндэксгейм"
        };

        public static readonly HashSet<string> yandexmapCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        { 
            "карты", "карту", "мапы", "мап", "map", "maps", "яндекскарты", "яндекскарту"
        };

        public static readonly HashSet<string> yandexweatherCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "яндекспогода", "яндекс погод", "яндекспогод", "яндекс погода", "яндекспогодка", "яндекспог", "яндекспогодка",
            "яндекспогода", "яндекспогода", "яндекс погода", "яндекс погодка", "яндекспогодка", "яндекспогода",
            "погода", "погодка", "погод", "погодка", "погода", "погода", "погод", "погодка", "погода"
        };

        public static readonly HashSet<string> yandexmailCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "yandexmail", "яндексмэил", "яндекспочту", "yandexmails", "яндекспочты", 
            "яндекспочта", "яндекспочтой", "яндекспочту", "яндексмайл", "яндексписьма", 
            "яндексписьмо", "яндекспочт", "яндекспочтами", "яндекспочтальный"
        };

        public static readonly HashSet<string> googlemailCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "googlemail", "гуглмэил", "гуглпочту", "googlemails", "gmail", "джимэйл", "джемейл"
        };

        public static readonly HashSet<string> vkmailCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "vkmail", "вэкамэил", "вкмэил", "вкпочту", "вэкапочту", "мэил", "мэйл", "vkmails", "mailru", "mail.ru", "мэилру"
        };
    }
}