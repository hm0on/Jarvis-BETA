using System;
using System.Collections.Generic;

public static class SystemCommandsListClass
{
    public static readonly HashSet<string> languageCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "язык", "языка"
    };
    
    public static readonly HashSet<string> tabCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "вкладка", "вкладку", "вкладки", "вкладке", "новуювкладку", "новаявкладка"
    };
    
    public static readonly HashSet<string> historyCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "история", "истории", "историю"
    };
    
    public static readonly HashSet<string> windowCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "окно", "окна", "акно"
    };
    
    public static readonly HashSet<string> downloadsCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "загрузки", "загрузок"
    };
    
    public static readonly HashSet<string> workCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "работу", "работы"
    };
    
    public static readonly HashSet<string> hibernateCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "спящийрежим", "перейтивспящийрежим", "переходвсон", "переходвспящийрежим", "перевестивспящийрежим"
    };
    
    
}