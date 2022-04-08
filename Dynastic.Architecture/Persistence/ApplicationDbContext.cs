using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Relationship>()
            .HasKey(e => new
            {
                e.PersonId,
                e.PartnerId
            });
        modelBuilder.Entity<Relationship>()
            .HasOne(p => p.Partner)
            .WithMany()
            .HasForeignKey(e => e.PartnerId);
        modelBuilder.Entity<Person>()
            .HasOne(p => p.Father)
            .WithOne();
        modelBuilder.Entity<Person>()
            .HasOne(p => p.Mother)
            .WithOne();
        modelBuilder.Entity<UserDynasty>()
            .HasKey(ud => new
            {
                UserId = ud.UserId,
                DynastyId = ud.DynastyId
            });
        modelBuilder.Entity<ChildRelationship>()
            .HasKey(cr => new
            {
                ChildId = cr.ChildId,
                RelationshipId = cr.RelationshipId
            });
    }

    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Dynasty> Dynasties => Set<Dynasty>();
    public DbSet<Relationship> Relationships => Set<Relationship>();
    public DbSet<UserDynasty> UserDynasties => Set<UserDynasty>();
}
