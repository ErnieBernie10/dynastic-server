using Dynastic.Application.Dynasties.Commands;
using Dynastic.Application.Persons.Commands;
using Dynastic.Application.Persons.Queries;
using Dynastic.Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dynastic.API.Controllers;

[Route("api/Dynasty/{dynastyId:guid}/[controller]")]
[Authorize]
public class PersonController : ApiControllerBase<PersonController>
{
    // GET: api/<PersonController>
    [HttpGet]
    public async Task<ActionResult<List<Person>>> Get(Guid dynastyId)
    {
        return await Mediator.Send(new GetPersonsByDynastyQuery() { DynastyId = dynastyId });
    }

    // GET api/<PersonController>/5
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Person>> Get(Guid dynastyId, Guid id)
    {
        return await Mediator.Send(new GetDynastyPersonByIdQuery { Id = id, DynastyId = dynastyId });
    }

    // POST api/<PersonController>
    [HttpPost]
    public async Task<ActionResult<Guid>> Post(Guid dynastyId, [FromBody] AddPersonToDynastyBody body)
    {
        var command = body.Adapt<AddPersonToDynastyCommand>();
        command.DynastyId = dynastyId;
        return await Mediator.Send(command);
    }

    // PUT api/<PersonController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<PersonController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}