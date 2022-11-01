using Dynastic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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


    public DatabaseFacade Database { get; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
