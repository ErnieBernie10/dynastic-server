using Dynastic.Application.Persons.Commands;
using Dynastic.Application.Persons.Queries;
using Dynastic.Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dynastic.API.Controllers;

[Route("api/Dynasty/{DynastyId:guid}/[controller]")]
[Authorize]
public class PersonController : ApiControllerBase<PersonController>
{
    // GET: api/<PersonController>
    [HttpGet]
    public async Task<ActionResult<List<Person>>> Get([FromRoute] GetPersonsByDynastyQuery request)
    {
        return await Mediator.Send(request);
    }

    // GET api/<PersonController>/5
    [HttpGet("{Id:guid}")]
    public async Task<ActionResult<Person>> Get([FromRoute] GetDynastyPersonByIdQuery request)
    {
        return await Mediator.Send(request);
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
