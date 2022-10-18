using Dynastic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Dynasty> Dynasties { get; }
    public DbSet<UserInfo> Users { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
