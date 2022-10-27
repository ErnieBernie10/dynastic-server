using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Common.Interfaces;
using Dynastic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dynastic.Application.Services;

public class AccessService : IAccessService
{
    public AccessService(ICurrentUserService currentUserService, IApplicationDbContext context)
    {
        _currentUserService = currentUserService;
        _context = context;
    }

    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;

    public bool HasAccessToDynasty(Dynasty dynasty)
    {
        return dynasty.OwnershipProperties.OwnerUserId != null &&
               (dynasty.OwnershipProperties.OwnerUserId.Equals(_currentUserService.UserId) ||
                dynasty.OwnershipProperties.Members.Contains(_currentUserService.UserId));
    }

    public IQueryable<Dynasty> GetUserDynasties()
    {
        // TODO: Refactor into some efcore expression once they support ARRAY_CONTAINS
        return _context.Dynasties.FromSqlRaw("SELECT * FROM c WHERE c.OwnershipProperties.OwnerUserId LIKE {0} OR ARRAY_CONTAINS(c.OwnershipProperties.Members, {0})", _currentUserService.UserId);
    }
}