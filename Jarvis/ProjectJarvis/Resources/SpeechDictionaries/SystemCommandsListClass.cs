using System;
using System.Collections.Generic;

public static class SystemCommandsListClass
{
    public static readonly HashSet<string> languageCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "язык", "языка"
    };

    public static readonly HashSet<string> tabCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "вкладка", "вкладку", "вкладки", "вкладке", "новуювкладку", "новаявкладка"
    };

    public static readonly HashSet<string> historyCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "история", "истории", "историю"
    };

    public static readonly HashSet<string> windowCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "окно", "окна", "акно"
    };

    public static readonly HashSet<string> downloadsCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "загрузки", "загрузок"
    };

    public static readonly HashSet<string> workCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "работу", "работы"
    };

    public static readonly HashSet<string> hibernateCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "спящийрежим", "перейтивспящийрежим", "переходвсон", "переходвспящийрежим", "перевестивспящийрежим"
    };
}