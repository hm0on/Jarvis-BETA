using System;
using System.Collections.Generic;

public static class ProgramListClass
{
    // СПИСКИ НАЗВАНИЯ ПРОГРАММ
    public static readonly HashSet<string> telegramCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "тг", "тэгэ", "телеграм", "телеграмм", "телегу", "телега", "тэгэшку", "telegram", "телеграмдесктоп", "telegramdesctop"
    };
    
    public static readonly HashSet<string> yandexCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "yandex", "яндекс", "яндекц", "яндэкс", "яндекс браузер", "yandex browser"
    };

    public static readonly HashSet<string> googleCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "google", "gogle", "gugel", "гугл", "гугел", "гугль", "гогл", "гугле", "гогле", "гоугл"
    };
    
    public static readonly HashSet<string> steamCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "steam", "стим", "stim", "стима", "стиме", "стимэ"
    };

    public static readonly HashSet<string> epicgamesCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "epicgames", "epikgames", "epicgams", "epikgams", "епикгеймс", "епикгейм", "епикгамес", 
        "епикгемс", "епигеймс", "епигейм", "эпик"
    };

    public static readonly HashSet<string> whatsappCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "whatsapp", "whatsap", "whatsup", "watsap", "watsapp", "вацап", "вацапп", "ватсап", "ватсапп", "вацап"
    };

    public static readonly HashSet<string> discordCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "дискорд", "discord", "дескорд", "дэскорд", "дискордик", "disord", "dissord", 
        "diskord", "dyscord", "дискорд", "дискорт", "дискор", "дискордь", "дискордт"
    };
    
    public static readonly HashSet<string> spotifyCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "spotify", "спотифай", "спотифайа", "спотик", "спотика", "спот", "споти"
    };
    
    public static readonly HashSet<string> microsoftedgeCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "microsoftedge", "edge", "эдж", "майкрасофтэдж", "майкрософтэдж"
    };
    
    public static readonly HashSet<string> yandexmusicCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "yandexmusic", "яндексмузыку", "яндексмузыки", "яндексмьюзик", "яндексмузыка"
    };
    
    public static readonly HashSet<string> winfilesexplorerCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "explorer", "filesexplorer", "эксплорер", "проводник", "проводника"
    };

    public static readonly HashSet<string> winsettingsCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "settings", "setting", " windowssettings", " windowssetting", "настройкивиндовс", "настройки", "настройка"
    };

    public static readonly HashSet<string> microsoftstoreCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "microsoftstore", "store", "майкрасофтстор", "стор", "майкрософтстор", "майкрософтсторе"
    };
    
    public static readonly HashSet<string> binCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "корзина", "корзину", "корзины"
    };
}