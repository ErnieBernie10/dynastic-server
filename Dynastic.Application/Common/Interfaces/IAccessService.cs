using Dynastic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dynastic.Application.Common.Interfaces;

public interface IAccessService
{
    public bool HasAccessToDynasty(Dynasty dynasty);
    
    public IQueryable<Dynasty> GetUserDynasties();
}