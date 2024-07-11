using System.IO;
using System.Text.Json;

namespace Jarvis.Project.Settings.ConfigurationManager;

public class SettingsManagerClass
{
    public SettingsManagerClass(string city)
    {
        this.City = city;
    }

    public SettingsManagerClass()
    {
    }
    public string City { get; set; }

    public void Save()
    {
        string json = JsonSerializer.Serialize(this);
        File.WriteAllText("settings.json", json);
    }

    public string Load()
    {
        if (File.Exists("settings.json"))
        {
            string json = File.ReadAllText("settings.json");
            string a = JsonSerializer.Deserialize<SettingsManagerClass>(json).City;
            return a;
        }
        return "None";
    }
}