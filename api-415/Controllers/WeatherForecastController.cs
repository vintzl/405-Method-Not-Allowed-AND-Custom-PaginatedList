using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_415.Libs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api_415.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private PaginatedList<WeatherForecast> _paginatedList;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            var rng = new Random();
            _paginatedList = PaginatedList<WeatherForecast>.Create(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Id = index,
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }), 1, 13);
        }

        [HttpGet]
        public PaginatedList<WeatherForecast> Get()
        {
            return _paginatedList;
        }

        [HttpGet("{id}")]
        public WeatherForecast Get(int id)
        {
            return _paginatedList.SingleOrDefault(e => e.Id == id);
        }

        [HttpPost]
        public ActionResult Add(WeatherForecast weatherForecasts)
        {
            if (ModelState.IsValid)
            {
                _paginatedList.SingleOrDefault(w => w.Id == weatherForecasts.Id).Summary = weatherForecasts.Summary;

                return NoContent();
            }
            return BadRequest(ModelState);
        }

        // Uncomment bellow returns:
        // System.NullReferenceException: Object reference not set to an instance of an object.
        // [HttpPost]
        // public ActionResult Add(PaginatedList<WeatherForecast> weatherForecasts)
        // {
        //     if (ModelState.IsValid)
        //     {


        //         return NoContent();
        //     }
        //     return BadRequest(ModelState);
        // }
    }
}
