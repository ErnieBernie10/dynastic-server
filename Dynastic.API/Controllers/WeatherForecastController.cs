using Dynastic.Application.Weatherforecasts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dynastic.API.Controllers;

public class WeatherForecastController : ApiControllerBase<WeatherForecastController>
{
    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<ActionResult<List<WeatherForecast>>> Get([FromQuery] GetWeatherForecastsQuery query)
    {
        return Ok(await Mediator.Send(query));
    }
}
