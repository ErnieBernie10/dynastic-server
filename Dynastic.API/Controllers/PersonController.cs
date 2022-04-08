using Dynastic.Application.Persons.Queries;
using Dynastic.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dynastic.API.Controllers;

[Route("api/Dynasty/{DynastyId}/[controller]")]
[Authorize]
public class PersonController : ApiControllerBase<PersonController>
{
    // GET: api/<PersonController>
    [HttpGet]
    public async Task<List<Person>> Get([FromRoute] GetPersonsByDynastyQuery request)
    {
        return await Mediator.Send(request);
    }

    // GET api/<PersonController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<PersonController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
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
