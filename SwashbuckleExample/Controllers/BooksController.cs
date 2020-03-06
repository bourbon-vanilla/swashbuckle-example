using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SwashbuckleExample.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
    //public class BooksController : ControllerBase
    //{
    //    private static readonly string[] Summaries = new[]
    //    {
    //        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    //    };

    //    private readonly ILogger<BooksController> _logger;

    //    public BooksController(ILogger<BooksController> logger)
    //    {
    //        _logger = logger;
    //    }

    //    [HttpGet]
    //    public IEnumerable<WeatherForecast> Get()
    //    {
    //        var rng = new Random();
    //        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //        {
    //            Date = DateTime.Now.AddDays(index),
    //            TemperatureC = rng.Next(-20, 55),
    //            Summary = Summaries[rng.Next(Summaries.Length)]
    //        })
    //        .ToArray();
    //    }

    //    /// <summary>
    //    /// Get specific forecast.
    //    /// </summary>
    //    /// <param name="forecastId">The id of the forecast to get.</param>
    //    /// <returns>The specific weather forecast.</returns>
    //    [HttpGet("forecastId")]
    //    public WeatherForecast Get(int forecastId)
    //    {
    //        var rng = new Random();
    //        return new WeatherForecast
    //        {
    //            Date = DateTime.Now.AddDays(forecastId),
    //            TemperatureC = rng.Next(-20, 55),
    //            Summary = Summaries[rng.Next(Summaries.Length)]
    //        };
    //    }

    //}
}
