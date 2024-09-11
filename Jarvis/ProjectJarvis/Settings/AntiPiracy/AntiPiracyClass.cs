using System;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace Jarvis.ProjectJarvis.Settings.AntiPiracy;

public static class AntiPiracyClass
{
    // Проверка на то, первый ли это запуск программы
    private const string RegistryKeyPath = @"SOFTWARE\LUX\Jarvis";
    private const string RegistryValueName = "HasRunBefore";

    private static async Task Main(string[] args)
    {
        await GetHwid_MBox_Y();
        await GetHwid_MBox_N();
    }

    public static bool IsFirstRun()
    {
        try
        {
            using (var key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
            {
                if (key != null)
                {
                    var value = key.GetValue(RegistryValueName);
                    if (value != null && value.ToString() == "1") return false; // Программа уже запускалась ранее
                }
                else
                {
                    using (var newKey = Registry.CurrentUser.CreateSubKey(RegistryKeyPath))
                    {
                        if (newKey != null) newKey.SetValue(RegistryValueName, "1");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при работе с реестром: " + ex.Message);
        }

        return true; // Первый запуск программы
    }


    public static async Task GetHwid_MBox_Y()
    {
        var uuid = GetUuid();
        if (uuid == null)
        {
            Console.WriteLine("Не удалось получить UUID");
            return;
        }

        var hwid = GetMd5Hash(uuid);
        var url =
            "https://docs.google.com/spreadsheets/d/1jz8hKO4EZfWou0uoSNBsG6EHkSe6-1LYDmzFcuS0dpw/export?format=csv";

        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
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
        var uuid = GetUuid();
        if (uuid == null)
        {
            Console.WriteLine("Не удалось получить UUID");
            return;
        }

        var hwid = GetMd5Hash(uuid);
        var url =
            "https://docs.google.com/spreadsheets/d/1jz8hKO4EZfWou0uoSNBsG6EHkSe6-1LYDmzFcuS0dpw/export?format=csv";

        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
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


    private static string GetUuid()
    {
        string uuid = null;
        try
        {
            var p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/C " + "wmic csproduct get uuid";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true; // Перенаправление ошибок
            p.Start();
            var output = p.StandardOutput.ReadToEnd();
            var errorOutput = p.StandardError.ReadToEnd(); // Считывает весь вывод ошибок
            p.WaitForExit();


            if (!string.IsNullOrEmpty(errorOutput))
            {
                Console.WriteLine("Ошибка при выполнении команды: " + errorOutput);
                return null;
            }

            // Console.WriteLine("Вывод команды: " + output);  // Вывод отладочной информации - ВКЛЮЧИТЬ ПРИ ОШИБКАХ


            // Разбиваем вывод на строки и ищем строку с UUID
            var lines = output.Trim().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length > 1) uuid = lines[1].Trim(); // Вторую строку
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при получении UUID: " + ex.Message);
        }

        return uuid;
    }

    private static string GetMd5Hash(string input)
    {
        using (var md5 = MD5.Create())
        {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            for (var i = 0; i < hashBytes.Length; i++) sb.Append(hashBytes[i].ToString("x2"));
            return sb.ToString();
        }
    }
}