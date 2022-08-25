using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Common.Interfaces;
using Dynastic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dynastic.Application.Common.Services;

public class AccessService : IAccessService
{
    public AccessService(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    private readonly ICurrentUserService _currentUserService;

    public bool HasAccessToDynasty(Dynasty dynasty)
    {
        return dynasty.OwnershipProperties.OwnerUserId != null &&
               (dynasty.OwnershipProperties.OwnerUserId.Equals(_currentUserService.UserId) ||
                dynasty.OwnershipProperties.Members.Contains(_currentUserService.UserId));
    }

    public IQueryable<Dynasty> FilterUserDynasties(DbSet<Dynasty> dynasties)
    {
        // TODO: Refactor into some efcore expression once they support ARRAY_CONTAINS
        return dynasties.FromSqlRaw("SELECT * FROM c WHERE c.OwnershipProperties.OwnerUserId LIKE {0} OR ARRAY_CONTAINS(c.OwnershipProperties.Members, {0})", _currentUserService.UserId);
    }
}