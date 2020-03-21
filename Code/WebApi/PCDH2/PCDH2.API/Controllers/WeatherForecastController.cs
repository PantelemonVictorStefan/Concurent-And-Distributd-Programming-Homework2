using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PCDH2.Core.Entities;
using PCDH2.Services.Contracts;
using PCDH2.Services.Implementations;
using PCDH2.Services.Models;

namespace Presentation.PCDH2.API.Controllers
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

        private readonly IMapper _mapper;

        protected IArticleService articleService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IMapper mapper, IArticleService articleService)
        {
            _logger = logger;
            _mapper = mapper;
            this.articleService = articleService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}

