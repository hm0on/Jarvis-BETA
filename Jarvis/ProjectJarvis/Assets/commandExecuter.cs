using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AudioSwitcher.AudioApi.CoreAudio;
using WindowsInput;
using WindowsInput.Native;

namespace Jarvis.Assets
{
    internal class commandExecuter
    {

        Assets.power power = new power(); // класс для работы с питанием
        Assets.shell32 shell32 = new shell32(); // класс для работы с корзиной
        InputSimulator simulator = new InputSimulator(); // для работы с имитацией клавиатуры

        public void execute(string vCommand)
        {
            // Вывод истории запросов пользователя
            string str = vCommand;
            string vCommand_and_First_Symbol_Up = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
            MainWindow.Instance.UpdateUserPhrazHistory(vCommand_and_First_Symbol_Up);

            if (string.IsNullOrEmpty(vCommand)) { return; }
            var defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice; // для работы со звуком

            if (vCommand == "очистка корзины")
            {
                shell32.CleanRecycleBin();
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "завершение работы")
            {
                power.Shutdown();
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "перезагрузка")
            {
                power.Restart();
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "спящий режим")
            {
                power.Hibernate();
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "скриншот")
            {

            }
            if (vCommand == "папка")
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Новая папка");
                if (!Directory.Exists(path))
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                        VoiceJarvis.JarvisVoice();
                    }
                    catch
                    {

                    }
                }
            }
            if (vCommand == "смена языка")
            {


                simulator.Keyboard.KeyDown(VirtualKeyCode.LSHIFT);
                simulator.Keyboard.KeyPress(VirtualKeyCode.MENU);
                simulator.Keyboard.KeyUp(VirtualKeyCode.LSHIFT);
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "удалить последнее")
            {
                simulator.Keyboard.KeyDown(VirtualKeyCode.LCONTROL);
                simulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
                simulator.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "удалить всё")
            {
                simulator.Keyboard.KeyDown(VirtualKeyCode.LCONTROL);
                simulator.Keyboard.KeyPress(VirtualKeyCode.VK_A);
                simulator.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);
                simulator.Keyboard.KeyPress(VirtualKeyCode.DELETE);
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "копировать")
            {
                simulator.Keyboard.KeyDown(VirtualKeyCode.LCONTROL);
                simulator.Keyboard.KeyPress(VirtualKeyCode.VK_C);
                simulator.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "вставить")
            {
                simulator.Keyboard.KeyDown(VirtualKeyCode.LCONTROL);
                simulator.Keyboard.KeyPress(VirtualKeyCode.VK_V);
                simulator.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "удалить")
            {
                simulator.Keyboard.KeyPress(VirtualKeyCode.DELETE);
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "выделить всё")
            {
                simulator.Keyboard.KeyDown(VirtualKeyCode.LCONTROL);
                simulator.Keyboard.KeyPress(VirtualKeyCode.VK_A);
                simulator.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "отправить")
            {
                simulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "открыть тг")
            {
                Process.Start(new ProcessStartInfo("https://web.telegram.org/k/") { UseShellExecute = true });
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "открыть вк")
            {
                Process.Start(new ProcessStartInfo("https://vk.com/") { UseShellExecute = true });
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "открыть ютуб")
            {
                Process.Start(new ProcessStartInfo("https://www.youtube.com/") { UseShellExecute = true });
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "открыть нетфликс")
            {
                Process.Start(new ProcessStartInfo("https://www.netflix.com/") { UseShellExecute = true });
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "громкость на максимум")
            {
                defaultPlaybackDevice.Volume = 100;
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "громкость на минимум")
            {
                defaultPlaybackDevice.Volume = 0;
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "громкость на середину")
            {
                defaultPlaybackDevice.Volume = 50;
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "сделать громче")
            {
                int old = (int)defaultPlaybackDevice.Volume;
                defaultPlaybackDevice.Volume = old + 15;
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "сделать тише")
            {
                int old = (int)defaultPlaybackDevice.Volume;
                defaultPlaybackDevice.Volume = old - 15;
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "завершение работы")
            {
                Environment.Exit(0);
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "открыть вкладку")
            {
                simulator.Keyboard.KeyDown(VirtualKeyCode.LCONTROL);
                simulator.Keyboard.KeyPress(VirtualKeyCode.VK_T);
                simulator.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "закрыть вкладку")
            {
                simulator.Keyboard.KeyDown(VirtualKeyCode.LCONTROL);
                simulator.Keyboard.KeyPress(VirtualKeyCode.F4);
                simulator.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "вернуть вкладку")
            {
                simulator.Keyboard.KeyDown(VirtualKeyCode.LCONTROL);
                simulator.Keyboard.KeyDown(VirtualKeyCode.LSHIFT);
                simulator.Keyboard.KeyPress(VirtualKeyCode.VK_T);
                simulator.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);
                simulator.Keyboard.KeyDown(VirtualKeyCode.LSHIFT);
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "добавить в закладки")
            {
                // нету горячих клавиш для такого
            }
            if (vCommand == "очистить историю")
            {
                simulator.Keyboard.KeyDown(VirtualKeyCode.LCONTROL);
                simulator.Keyboard.KeyDown(VirtualKeyCode.LSHIFT);
                simulator.Keyboard.KeyPress(VirtualKeyCode.DELETE);
                simulator.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);
                simulator.Keyboard.KeyDown(VirtualKeyCode.LSHIFT);
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand.Contains("поиск"))
            {
                string search = vCommand.Replace("поиск ", "");
                Process.Start(new ProcessStartInfo($"https://yandex.ru/search/?text={search}") { UseShellExecute = true });
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "обновить")
            {
                simulator.Keyboard.KeyDown(VirtualKeyCode.LCONTROL);
                simulator.Keyboard.KeyPress(VirtualKeyCode.VK_R);
                simulator.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "открыть загрузки")
            {
                simulator.Keyboard.KeyDown(VirtualKeyCode.LCONTROL);
                simulator.Keyboard.KeyPress(VirtualKeyCode.VK_J);
                simulator.Keyboard.KeyUp(VirtualKeyCode.LCONTROL);
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "закрыть окно")
            {
                simulator.Keyboard.KeyDown(VirtualKeyCode.LMENU);
                simulator.Keyboard.KeyPress(VirtualKeyCode.F4);
                simulator.Keyboard.KeyUp(VirtualKeyCode.LMENU);
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "свернуть окно")
            {
                simulator.Keyboard.KeyDown(VirtualKeyCode.LWIN);
                simulator.Keyboard.KeyPress(VirtualKeyCode.VK_M);
                simulator.Keyboard.KeyUp(VirtualKeyCode.LWIN);
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "другое окно")
            {
                simulator.Keyboard.KeyDown(VirtualKeyCode.LMENU);
                simulator.Keyboard.KeyPress(VirtualKeyCode.TAB);
                simulator.Keyboard.KeyUp(VirtualKeyCode.LMENU);
                VoiceJarvis.JarvisVoice();
            }
            if (vCommand == "развернуть окно")
            {
                simulator.Keyboard.KeyDown(VirtualKeyCode.LWIN);
                simulator.Keyboard.KeyDown(VirtualKeyCode.LSHIFT);
                simulator.Keyboard.KeyPress(VirtualKeyCode.VK_M);
                simulator.Keyboard.KeyUp(VirtualKeyCode.LWIN);
                simulator.Keyboard.KeyDown(VirtualKeyCode.LSHIFT);
                VoiceJarvis.JarvisVoice();
            }

        }
    }
}
