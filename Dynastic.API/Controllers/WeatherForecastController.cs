using Dynastic.Application.Weatherforecasts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dynastic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ISender _mediator;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ISender mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<ActionResult<List<WeatherForecast>>> Get([FromQuery] GetWeatherForecastsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}