using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Common.Interfaces;
using Dynastic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dynastic.Application.Services;

public class UserInfoService : IUserInfoService
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UserInfoService(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UserInfo> GetUserInfo()
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserId.Equals(_currentUserService.UserId)) ??
               throw new ArgumentException("User not found");
    }
}