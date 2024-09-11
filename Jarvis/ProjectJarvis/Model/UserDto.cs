using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Jarvis.ProjectJarvis.Model;

public class AuthUserDto
{
    [Required]
    [MaxLength(24), MinLength(6)]
    public string Username { get; set; }

    [Required]
    [MaxLength(24), MinLength(6)]
    public string Password { get; set; }
}

public class UserDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; }

    public DateTime? ExpiredTime { get; set; }
}