using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SS.Template.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
#pragma warning disable CA1822 // Mark members as static
        public IEnumerable<WeatherForecast> Get()
#pragma warning restore CA1822 // Mark members as static
        {
            var rng = new Random();
            var temperatures = rng.Next(-20, 55);
            var summary = Summaries[rng.Next(Summaries.Length)];

            _logger.LogDebug("Random temperatures generated:", temperatures);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = temperatures,
                Summary = summary
            })
            .ToArray();
        }
    }
}
