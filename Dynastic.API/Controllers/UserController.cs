using Dynastic.Application.Users.Commands;
using Dynastic.Application.Users.Queries;
using Dynastic.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dynastic.API.Controllers;

[Authorize]
public class UserController : ApiControllerBase<UserController>
{
    [HttpGet]
    public async Task<UserInfoDto> GetUserInfo()
    {
        return await Mediator.Send(new GetUserInfoQuery());
    }

    [HttpPost]
    public async Task<Guid> CompleteSignup([FromBody] CompleteSignupCommand body)
    {
        return await Mediator.Send(body);
    }
}