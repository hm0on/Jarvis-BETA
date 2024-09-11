using System;
using System.Collections.Generic;

public static class ProgramListClass
{
    // СПИСКИ НАЗВАНИЯ ПРОГРАММ
    public static readonly HashSet<string> telegramCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "тг", "тэгэ", "телеграм", "телеграмм", "телегу", "телега", "тэгэшку", "telegram", "телеграмдесктоп",
        "telegramdesctop"
    };

    public static readonly HashSet<string> yandexCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "yandex", "яндекс", "яндекц", "яндэкс", "яндекс браузер", "yandex browser"
    };

    public static readonly HashSet<string> googleCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "google", "gogle", "gugel", "гугл", "гугел", "гугль", "гогл", "гугле", "гогле", "гоугл"
    };

    public static readonly HashSet<string> steamCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "steam", "стим", "stim", "стима", "стиме", "стимэ"
    };

    public static readonly HashSet<string> epicgamesCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "epicgames", "epikgames", "epicgams", "epikgams", "епикгеймс", "епикгейм", "епикгамес",
        "епикгемс", "епигеймс", "епигейм", "эпик"
    };

    public static readonly HashSet<string> whatsappCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "whatsapp", "whatsap", "whatsup", "watsap", "watsapp", "вацап", "вацапп", "ватсап", "ватсапп", "вацап"
    };

    public static readonly HashSet<string> discordCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "дискорд", "discord", "дескорд", "дэскорд", "дискордик", "disord", "dissord",
        "diskord", "dyscord", "дискорд", "дискорт", "дискор", "дискордь", "дискордт"
    };

    public static readonly HashSet<string> spotifyCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "spotify", "спотифай", "спотифайа", "спотик", "спотика", "спот", "споти"
    };

    public static readonly HashSet<string> microsoftedgeCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "microsoftedge", "edge", "эдж", "майкрасофтэдж", "майкрософтэдж"
    };

    public static readonly HashSet<string> yandexmusicCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "yandexmusic", "яндексмузыку", "яндексмузыки", "яндексмьюзик", "яндексмузыка"
    };

    public static readonly HashSet<string> winfilesexplorerCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "explorer", "filesexplorer", "эксплорер", "проводник", "проводника"
    };

    public static readonly HashSet<string> winsettingsCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "settings", "setting", " windowssettings", " windowssetting", "настройкивиндовс", "настройки", "настройка"
    };

    public static readonly HashSet<string> microsoftstoreCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "microsoftstore", "store", "майкрасофтстор", "стор", "майкрософтстор", "майкрософтсторе"
    };

    public static readonly HashSet<string> binCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "корзина", "корзину", "корзины"
    };
}