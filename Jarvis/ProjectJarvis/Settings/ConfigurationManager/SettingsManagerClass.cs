using System.IO;
using System.Text.Json;

namespace Jarvis.ProjectJarvis.Settings.ConfigurationManager;

public class SettingsManagerClass
{
    public SettingsManagerClass(string city)
    {
        City = city;
    }

    public SettingsManagerClass()
    {
    }

    public string City { get; set; }

    public void Save()
    {
        var json = JsonSerializer.Serialize(this);
        File.WriteAllText("settings.json", json);
    }

    public string Load()
    {
        if (File.Exists("settings.json"))
        {
            var json = File.ReadAllText("settings.json");
            var a = JsonSerializer.Deserialize<SettingsManagerClass>(json).City;
            return a;
        }

        return "None";
    }
}