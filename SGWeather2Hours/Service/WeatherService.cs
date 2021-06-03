using SGWeather2Hours.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using static SGWeather2Hours.Models.ForecastModel;

namespace SGWeather2Hours.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            SetClient();
        }

        public async Task<List<Forecast>> GetWeather()
        {
            SetClient();
            string queryToAppend = BuildQuery();
            return await GetFromApi(queryToAppend);
        }
        public void SetClient()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public string BuildQuery()
        {
            string queryDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            return "date_time" + queryDateTime;
        }

        public async Task<List<Forecast>> GetFromApi(string query)
        {
            List<Forecast> forecast = new List<Forecast>();
            UriBuilder baseUri = new UriBuilder("https://api.data.gov.sg/v1/environment/2-hour-weather-forecast");
            baseUri.Query = query; // no need ? for .Net5. auto insert if not found

            HttpResponseMessage response = await _httpClient.GetAsync(baseUri.Uri);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Weather weather = JsonSerializer.Deserialize<Weather>(content);

                if (weather.api_info.status == "healthy")
                {
                    forecast = weather.items[0].forecasts;
                }
            }
            return forecast;
        }
    }
}

