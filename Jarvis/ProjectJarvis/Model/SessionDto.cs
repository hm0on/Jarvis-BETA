using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Jarvis.ProjectJarvis.Model;

public class SessionDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public DateTime LastUsage { get; set; }
    
    [Required]
    public string Token { get; set; }

    [Required]
    public int SessionUserId { get; set; }

    public SessionDto(int id, DateTime lastUsage, string token, int sessionUserId)
    {
        Id = id;
        LastUsage = lastUsage;
        Token = token;
        SessionUserId = sessionUserId;
    }

    public SessionDto GetSession()
    {
        var session = JsonConvert.DeserializeObject<SessionDto>(File.ReadAllText("session.json"));
        return session!;
    }

    public void Delete()
    {
        File.Delete("session.json");
    }
}
