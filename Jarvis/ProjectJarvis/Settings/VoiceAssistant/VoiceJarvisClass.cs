using System;
using System.IO;
using System.Media;
using System.Reflection;
using Jarvis.Project.Views.Pages.Primary;

namespace Jarvis.ProjectJarvis.Settings.VoiceAssistant;

internal class VoiceJarvisClass
{
    private static string MakePathToVoiceFile(string nameVoice)
    {
        // Получаем текущую директорию исполняемого файла
        var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        // Поднимаемся на два уровня вверх от текущей директории
        var projectDir = Directory.GetParent(appDir).Parent.Parent.FullName;

        // Относительный путь к файлу
        var relativePath = @"ProjectJarvis\Resources\AssistantVoices\" + nameVoice;
        // Комбинируем проектную директорию с относительным путем
        var fullPath = Path.Combine(projectDir, relativePath);

        return fullPath;
    }

    public static void JarvisVoiceComplete()
    {
        var complete = new SoundPlayer(MakePathToVoiceFile("Всегда к вашим услугам сэр.wav"));
        Basic.Instance.AddJarvisAnswer("Всегда к Вашим услугам, сэр!");
        complete.Play();
    }

    public static void JarvisVoiceYes()
    {
        var rnd = new Random();
        var jarvisVoiceAnswer = rnd.Next(1, 5);

        switch (jarvisVoiceAnswer)
        {
            case 1:
                //Basic.UpdateJarvisPhrazHistory("Есть!"); <-- временно выключено
                var yes = new SoundPlayer(MakePathToVoiceFile("Есть.wav"));
                Basic.Instance.AddJarvisAnswer("Есть!");
                yes.Play();
                break;
            case 2:
                //Basic.UpdateJarvisPhrazHistory("Да, сэр!");
                var yessir2 = new SoundPlayer(MakePathToVoiceFile("Да сэр(второй).wav"));
                Basic.Instance.AddJarvisAnswer("Да, сэр!");
                yessir2.Play();
                break;
            case 3:
                //Basic.UpdateJarvisPhrazHistory("Да, сэр!");
                var yessir = new SoundPlayer(MakePathToVoiceFile("Да сэр.wav"));
                Basic.Instance.AddJarvisAnswer("Да, сэр!");
                yessir.Play();
                break;
            case 4:
                //Basic.UpdateJarvisPhrazHistory("Запрос выполнен, сэр!");
                var comletesir = new SoundPlayer(MakePathToVoiceFile("Запрос выполнен сэр.wav"));
                Basic.Instance.AddJarvisAnswer("Запрос выполнен, сэр!");
                comletesir.Play();
                break;
            case 5:
                //Basic.UpdateJarvisPhrazHistory("Загружаю, сэр!");
                var loadingsir = new SoundPlayer(MakePathToVoiceFile("Загружаю сэр.wav"));
                Basic.Instance.AddJarvisAnswer("Загружаю, сэр!");
                loadingsir.Play();
                break;
        }
    }
}