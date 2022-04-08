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
    public async Task<IEnumerable<Dynasty>> Get([FromRoute] GetDynastiesForUserQuery request)
    {
        return await Mediator.Send(request);
    }

    // GET api/<DynastyController>/5
    [HttpGet("{Id}")]
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

    // PUT api/<DynastyController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<DynastyController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
