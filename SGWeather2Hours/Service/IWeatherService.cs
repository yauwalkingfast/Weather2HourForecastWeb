using SGWeather2Hours.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SGWeather2Hours.Models.ForecastModel;

namespace SGWeather2Hours.Service
{
    public interface IWeatherService
    {
        public Task<List<Forecast>> GetWeather();
        public void SetClient();
        public string BuildQuery();
        public Task<List<Forecast>> GetFromApi(string query);
    }
}
