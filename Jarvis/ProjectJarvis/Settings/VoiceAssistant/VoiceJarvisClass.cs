using System;
using System.Media;
using Jarvis.Project.Views.Pages;

namespace Jarvis.Project.Settings.VoiceAssistant
{
    class VoiceJarvisClass
    {

        public static void JarvisVoiceYes()
        {
            Random rnd = new Random();
            int Jarvis_Voice_answer = rnd.Next(1, 5);

            switch (Jarvis_Voice_answer)
            {
                case 1:
                    //Basic.UpdateJarvisPhrazHistory("Есть!");
                    var yes = new SoundPlayer(@"C:\Program Files\Jarvis\JarvisVoice\Есть.wav");
                    yes.Play();
                    break;
                case 2:
                    //Basic.UpdateJarvisPhrazHistory("Да, сэр!");
                    var yessir2 = new SoundPlayer(@"C:\Program Files\Jarvis\JarvisVoice\Да сэр(второй).wav");
                    yessir2.Play();
                    break;
                case 3:
                    //Basic.UpdateJarvisPhrazHistory("Да, сэр!");
                    var yessir = new SoundPlayer(@"C:\Program Files\Jarvis\JarvisVoice\Да сэр.wav");
                    yessir.Play();
                    break;
                case 4:
                    //Basic.UpdateJarvisPhrazHistory("Запрос выполнен, сэр!");
                    var comletesir = new SoundPlayer(@"C:\Program Files\Jarvis\JarvisVoice\Запрос выполнен сэр.wav");
                    comletesir.Play();
                    break;
                case 5:
                    //Basic.UpdateJarvisPhrazHistory("Загружаю, сэр!");
                    var loadingsir = new SoundPlayer(@"C:\Program Files\Jarvis\JarvisVoice\Загружаю сэр.wav");
                    loadingsir.Play();
                    break;

            }
        }
    }
}
