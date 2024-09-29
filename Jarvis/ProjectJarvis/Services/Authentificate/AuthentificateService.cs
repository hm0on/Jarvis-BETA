using Jarvis.ProjectJarvis.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;

namespace Jarvis.ProjectJarvis.Services.Authentificate
{
    public class AuthentificateService: IAuthentificateService
    {
        private const string Url = "http://127.0.0.1:5149/";
        private static readonly HttpClient Client = new() { BaseAddress = new Uri(Url) };

        public SessionDto Register(AuthUserDto userDto)
        {
            var content = JsonContent.Create(userDto);
            using var response = Client.PostAsync("api/account/register", content).Result;
            if (response.IsSuccessStatusCode)
            {
                var session = response.Content.ReadFromJsonAsync<SessionDto>().Result!;
                return session;
            }
            else
            {
                throw new Exception("Error: " + response.ReasonPhrase);
            }
        }

        public SessionDto Login(AuthUserDto userDto)
        {
            var content = JsonContent.Create(userDto);
            using var response = Client.PostAsync("api/account/login", content).Result;
            if (response.IsSuccessStatusCode)
            {
                var a = response.Content.ReadAsStringAsync().Result;
                var session = response.Content.ReadFromJsonAsync<SessionDto>().Result!;
                return session;
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }

        public UserDto GetMe(SessionDto sessionDto)
        {
            var token = sessionDto.Token;
            var response = Client.GetAsync($"api/account/getme?sessionToken={token}").Result;
            if (response.IsSuccessStatusCode)
            {
                var userStr = response.Content.ReadAsStringAsync().Result!;
                var user = JsonConvert.DeserializeObject<UserDto>(userStr)!;
                return user;
            }
            else
            {
                throw new Exception("Error: " + response.ReasonPhrase);
            }
        }

        public void AddKey(SessionDto session, string key)
        {
            var response = Client.
                PostAsync($"api/account/setkey?sessionToken={session.Token}&key={key}", new StringContent("")).Result;
            if (response.IsSuccessStatusCode) return;
            switch (response.StatusCode.ToString())
            {
                case "404":
                    throw new Exception("Ключ не найден");
                case "400":
                    throw new Exception("Ключ не найден");
                case "410":
                    throw new Exception("Ваша сессия не активна");
            }
            throw new Exception("Error: " + response.ReasonPhrase);
        }

        public void Logout(SessionDto session)
        {
            var response = Client.
                DeleteAsync($"api/Account/logout?sessionToken={session.Token}").Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error: " + response.ReasonPhrase);
            }

        }
    }
}
