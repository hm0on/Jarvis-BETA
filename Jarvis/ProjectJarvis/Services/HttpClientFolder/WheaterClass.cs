using System.Net.Http;
using System.Threading.Tasks;

namespace Jarvis.Project.Services.HttpClientFolder
{
    public class WheaterClass
    {
        private static readonly string APIKEY = Properties.Settings.Default.APIkeyWeather;

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
