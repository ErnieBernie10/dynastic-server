using Dynastic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Dynasty> Dynasties { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<UserDynasty> UserDynasties { get; set; }
    }
}
