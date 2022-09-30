using Dynastic.Application.Dynasties.Commands;
using Dynastic.Application.Dynasties.Queries;
using Dynastic.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dynastic.API.Controllers;

[Authorize]
public class DynastyController : ApiControllerBase<DynastyController>
{
    // GET: api/<DynastyController>
    [HttpGet]
    public async Task<IEnumerable<Dynasty>> Get([FromQuery] GetDynastiesForUserQuery request)
    {
        return await Mediator.Send(request);
    }

    // GET api/<DynastyController>/5
    [HttpGet("{Id:guid}")]
    public async Task<Dynasty> Get([FromRoute] GetDynastyByIdQuery query)
    {
        return await Mediator.Send(query);
    }

    // POST api/<DynastyController>
    [HttpPost]
    public async Task<Guid> Post([FromBody] AddDynastyCommand command)
    {
        return await Mediator.Send(command);
    }

    // POST api/<DynastyController>
    [HttpPut("{id:guid}/UploadCoaFile")]
    public async Task<Guid> UploadCoa(Guid id, [FromForm] CoaFileCommand command)
    {
        return await Mediator.Send(new AddDynastyCoaFileCommand() { Coa = command.Coa, Id = id });
    }

    // POST api/<DynastyController>
    [HttpPut("{id:guid}/CoaConfiguration")]
    public async Task<Guid> AddCoaConfiguration(Guid id, [FromBody] AddDynastyCoaConfigurationBody body)
    {
        return await Mediator.Send(new AddDynastyCoaConfigurationCommand() {
            CoaConfiguration = body.CoaConfiguration, Id = id
        });
    }

    // PUT api/<DynastyController>/5
    [HttpPut("{id:guid}")]
    public async Task<Guid> Put(Guid id, [FromBody] UpdateDynastyCommandBody body)
    {
        return await Mediator.Send(new UpdateDynastyCommand() {
            Id = id, Description = body.Description, Motto = body.Motto, Name = body.Name
        });
    }

    // DELETE api/<DynastyController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}