using System;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using log4net;

namespace Jarvis.Project.Settings.AntiPiracy
{
    public static class AntiPiracyClass
    {
        
        static async Task Main(string[] args)
        {
            await GetHwid_MBox_Y();
            await GetHwid_MBox_N();
        }

        // Проверка на то, первый ли это запуск программы
        private const string RegistryKeyPath = @"SOFTWARE\LUX\Jarvis";
        private const string RegistryValueName = "HasRunBefore";

        public static bool IsFirstRun()
        {
            
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
                {
                    if (key != null)
                    {
                        object value = key.GetValue(RegistryValueName);
                        if (value != null && value.ToString() == "1")
                        {
                            return false;  // Программа уже запускалась ранее
                        }
                    }
                    else
                    {
                        using (RegistryKey newKey = Registry.CurrentUser.CreateSubKey(RegistryKeyPath))
                        {
                            if (newKey != null)
                            {
                                newKey.SetValue(RegistryValueName, "1");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при работе с реестром: " + ex.Message);
            }
            return true;  // Первый запуск программы
        }



        public static async Task GetHwid_MBox_Y()
        {
            string uuid = GetUuid();
            if (uuid == null)
            {
                Console.WriteLine("Не удалось получить UUID");
                return;
            }

            string hwid = GetMd5Hash(uuid);
            string url = "https://docs.google.com/spreadsheets/d/1jz8hKO4EZfWou0uoSNBsG6EHkSe6-1LYDmzFcuS0dpw/export?format=csv";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    if (data.Contains(uuid))
                    {
                        MessageBox.Show("Защита от пиратства пройдена!");
                    }
                    else
                    {
                        Application.Current.Shutdown();
                        MessageBox.Show("Похоже, что ваш ПК не зарегистрирован! Программа не будет запущена.");
                    }
                }
                else
                {
                    Console.WriteLine("Не удалось получить данные");
                }
            }
        }

        public static async Task GetHwid_MBox_N()
        {
            string uuid = GetUuid();
            if (uuid == null)
            {
                Console.WriteLine("Не удалось получить UUID");
                return;
            }

            string hwid = GetMd5Hash(uuid);
            string url = "https://docs.google.com/spreadsheets/d/1jz8hKO4EZfWou0uoSNBsG6EHkSe6-1LYDmzFcuS0dpw/export?format=csv";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    if (data.Contains(uuid))
                    {
                        // Ничего не выполняем что бы не мешать пользователю
                    }
                    else
                    {
                        Application.Current.Shutdown();
                        MessageBox.Show("Похоже, что ваш ПК не зарегистрирован! Программа не будет запущена.");
                    }
                }
                else
                {
                    Application.Current.Shutdown();
                    MessageBox.Show("Не удалось получить данные");
                }
            }
        }


        static string GetUuid()
        {
            string uuid = null;
            try
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = "/C " + "wmic csproduct get uuid";
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;  // Перенаправление ошибок
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                string errorOutput = p.StandardError.ReadToEnd();  // Считывает весь вывод ошибок
                p.WaitForExit();


                if (!string.IsNullOrEmpty(errorOutput))
                {
                    Console.WriteLine("Ошибка при выполнении команды: " + errorOutput);
                    return null;
                }

                // Console.WriteLine("Вывод команды: " + output);  // Вывод отладочной информации - ВКЛЮЧИТЬ ПРИ ОШИБКАХ


                // Разбиваем вывод на строки и ищем строку с UUID
                string[] lines = output.Trim().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length > 1)
                {
                    uuid = lines[1].Trim();  // Вторую строку
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при получении UUID: " + ex.Message);
            }
            return uuid;
        }

        static string GetMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
