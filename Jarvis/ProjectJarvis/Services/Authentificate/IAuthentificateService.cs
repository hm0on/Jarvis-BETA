using Jarvis.ProjectJarvis.Model;

namespace Jarvis.ProjectJarvis.Services.Authentificate;

public interface IAuthentificateService
{
    SessionDto Register(AuthUserDto userDto);
    SessionDto Login(AuthUserDto userDto);
    UserDto GetMe(SessionDto sessionDto);
    void AddKey(SessionDto session, string key);
    void Logout(SessionDto session);
}
