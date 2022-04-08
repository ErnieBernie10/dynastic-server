using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dynastic.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase<T> : ControllerBase where T : ApiControllerBase<T>
{
    private ILogger<T>? _logger;
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    protected ILogger<T> Logger => _logger ??= HttpContext.RequestServices.GetRequiredService<ILogger<T>>();
}
