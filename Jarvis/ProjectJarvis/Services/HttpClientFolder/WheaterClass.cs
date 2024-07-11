using System.Net.Http;
using System.Threading.Tasks;

namespace Jarvis.Project.Services.HttpClientFolder
{
    public class WheaterClass
    {
        private const string APIKEY = "e327b05bf5e93f0560363f8bc007f7e2";

        public static async Task<HttpClient> GetWeatherAsync(string city)
        {
            if (city == "None")
            {
                return new HttpClient();
            }
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={APIKEY}&units=metric&lang=ru";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).Result; // Получаем ответ от сервера
            }

            return null;
        }
    }
}
