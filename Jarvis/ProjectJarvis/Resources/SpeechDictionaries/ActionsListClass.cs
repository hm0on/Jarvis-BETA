using System;
using System.Collections.Generic;

public static class ActionsListClass
{
        public static readonly HashSet<string> openCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "открой", "открыть", "запусти", "запустить", "зайди в", "зайти в", "вруби", "врубить", "мне нужен", "будь добр открой", "выведи"
        };

        // Действия на закрытие
        public static readonly HashSet<string> closeCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "убери", "закрой", "убрать", "закрыть", "отруби", "отрубить"
        };

        // Действия на свертывание
        public static readonly HashSet<string> minimizeCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "сверни", "свернуть", "скрой", "скрыть", "минимализируй"
        };

        // Действия на развертывание
        public static readonly HashSet<string> upCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "разверни", "развернуть", "развернуться"
        };

        // Действия на развертывание на весь экран
        public static readonly HashSet<string> maximizeCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "на весь экран", "разверни на весь экран", "открой на весь экран", "максимализируй", "развернуть на весь экран"
        };
        
        // Действия на переключение на дргой объект
        public static readonly HashSet<string> otherCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "другое", "другие", "другой"
        };
        
        // СИСТЕМНЫЕ КОМАНДЫ
        // Действия на очистку
        public static readonly HashSet<string> clearCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "очичтить", "очисти", "очистка"
        };
        
        public static readonly HashSet<string> restoreCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "верни", "вернуть", "возвращение"
        };
        
        public static readonly HashSet<string> shutdownCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "звершить", "завершение", "заверши"
        };
        
        public static readonly HashSet<string> restartCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "перезагрузить", "перезагрузка", "выполниперезагрузку", "началоперезагрузки", "перезагрузипк", "перезагрузикомпьютер", "перезагрузитькомпьютер"
        };
        
        public static readonly HashSet<string> changeCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "сменить", "смена", "поменять", "изменить"
        };
        
        public static readonly HashSet<string> volumecontrolCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "сделатьгромкость", "громкость", "изменитьгромкость", "установитьгромкость"
        };
        
        public static readonly HashSet<string> makeCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "сделать", "сделай"
        };
        
        public static readonly HashSet<string> volumemaxCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "максимум", "намаксимум", "максимальную", "максимальная", "намаксиммальную", "сто", "насто"
        };
        
        public static readonly HashSet<string> volumemediumCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "среднее", "насередину", "середина", "средняя", "насреднюю", "на50", "напятьдесят", "наполовину", "половина", "половину"
        };
        
        public static readonly HashSet<string> volumelowCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "минимальная", "минимум", "наминимум", "наминимальную", "наноль", "ноль"
        };
        
        public static readonly HashSet<string> volumeaddCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "громче", "погромче", "повыше", "выше" 
        };
        
        public static readonly HashSet<string> volumedelCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "тише", "ниже", "потише", "пониже"
        };
}