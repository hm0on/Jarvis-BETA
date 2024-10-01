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
    public Guid SessionUserId { get; set; }

    public SessionDto(int id, DateTime lastUsage, string token, Guid sessionUserId)
    {
        Id = id;
        LastUsage = lastUsage;
        Token = token;
        SessionUserId = sessionUserId;
    }

    public static SessionDto GetSession()
    {
        if (File.Exists("session.json"))
        {
            var session = JsonConvert.DeserializeObject<SessionDto>(File.ReadAllText("session.json"));
            return session;
        }
        else
        {
            throw new ValidationException();
        }
    }

    public void Delete()
    {
        File.Delete("session.json");
    }
}
