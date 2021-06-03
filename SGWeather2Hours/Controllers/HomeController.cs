using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SGWeather2Hours.Models;
using SGWeather2Hours.Service;
using System.Collections.Generic;
using System.Diagnostics;

using static SGWeather2Hours.Models.ForecastModel;

namespace SGWeather2Hours.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWeatherService _weatherSvc;

        public HomeController(IWeatherService weatherSvc, ILogger<HomeController> logger)
        {
            _weatherSvc = weatherSvc;
            _logger = logger;
        }

        public IActionResult Index()
        {

            List<Forecast> forecastAllLocations = _weatherSvc.GetWeather().Result;
            ViewData["forecast"] = forecastAllLocations;
            return View();
        }

        [HttpPost]
        public IActionResult GetAreaForecast([FromBody] Forecast fc)
        {
            if (ModelState.IsValid)
            {
                //string selectedLocation = Request.Form["selected"];
                string selectedLocation = fc.area;
                List<Forecast> forecastAllLocations = _weatherSvc.GetWeather().Result;

                string locationWeatherForecast = string.Empty;
                foreach (Forecast forecast in forecastAllLocations)
                {
                    if (forecast.area == selectedLocation)
                    {
                        locationWeatherForecast = forecast.forecast;
                    }
                }

                return Json(new
                {
                    status = "success",
                    message = locationWeatherForecast,
                    area = selectedLocation
                });
            }
            return Json(new
            {
                status = "error",
                message = "Service not available"
            });
        }
        
        // ----------------------------------- Scaffold ------------------------------------------- //
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    
}
