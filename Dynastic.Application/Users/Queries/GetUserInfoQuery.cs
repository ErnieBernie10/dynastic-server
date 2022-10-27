using Ardalis.GuardClauses;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Common.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dynastic.Application.Users.Queries;

public class GetUserInfoQuery : IRequest<UserInfoDto>
{
}

public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, UserInfoDto>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;

    public GetUserInfoQueryHandler(ICurrentUserService currentUserService, IApplicationDbContext context)
    {
        _currentUserService = currentUserService;
        _context = context;
    }

    public async Task<UserInfoDto> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId.Equals(_currentUserService.UserId),
                cancellationToken: cancellationToken);

        Guard.Against.NotFound(_currentUserService.UserId, userInfo, nameof(userInfo));

        return userInfo.Adapt<UserInfoDto>();
    }
}