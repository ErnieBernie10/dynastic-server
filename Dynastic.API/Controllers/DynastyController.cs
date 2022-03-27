using Dynastic.Application.Dynasties.Queries;
using Dynastic.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dynastic.API.Controllers
{
    [Authorize]
    public class DynastyController : ApiControllerBase<DynastyController>
    {
        // GET: api/<DynastyController>
        [HttpGet]
        public async Task<IEnumerable<Dynasty>> Get()
        {
            return await Mediator.Send(new GetDynastiesForUserQuery() { UserId = User.Identity?.Name });
        }

        // GET api/<DynastyController>/5
        [HttpGet("{id}")]
        public async Task<Dynasty> Get([FromRoute] GetDynastyByIdQuery query)
        {
            return await Mediator.Send(query);
        }

        // POST api/<DynastyController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
}
