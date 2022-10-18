using Dynastic.Application.Common.Interfaces;
using Dynastic.Application.Users.Queries;
using Dynastic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace Dynastic.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dynasty>(builder =>
            builder.Property(prop => prop.CoaConfiguration).HasConversion(
                v => JsonSerializer.Serialize(v,
                    new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }),
                v => JsonSerializer.Deserialize<JsonDocument>(v,
                    new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull })!));
        // modelBuilder.Entity<Relationship>()
        //     .ToContainer(nameof(Relationships))
        //     .HasKey(e => new {
        //         e.PersonId,
        //         e.PartnerId
        //     });
        // modelBuilder.Entity<Relationship>()
        //     .ToContainer(nameof(Relationships))
        //     .HasOne(p => p.Partner)
        //     .WithMany()
        //     .HasForeignKey(e => e.PartnerId);

        // modelBuilder.Entity<Person>()
        //     .ToContainer(nameof(Persons))
        //     .OwnsMany(p => p.Relationships)
        //     .HasOne(p => p.Relationship);
        // modelBuilder.Entity<Person>()
        //     .HasOne(p => p.Father)
        //     .WithOne();
        // modelBuilder.Entity<Person>()
        //     .HasOne(p => p.Mother)
        //     .WithOne();

        // modelBuilder.Entity<UserDynasty>()
        //     .ToContainer(nameof(UserDynasties))
        //     .HasKey(ud => new {
        //         UserId = ud.UserId,
        //         DynastyId = ud.DynastyId
        //     });

        // modelBuilder.Entity<ChildRelationship>()
        //     .ToContainer(nameof(Relationships))
        //     .HasKey(cr => new {
        //         ChildId = cr.ChildId,
        //         RelationshipId = cr.RelationshipId
        //     });
        modelBuilder.Entity<Dynasty>()
            .ToContainer(nameof(Dynasties))
            .HasNoDiscriminator()
            .OwnsMany(d => d.Members)
            .OwnsMany(m => m.Relationships);
        modelBuilder.Entity<UserInfo>()
            .ToContainer(nameof(Users))
            .HasNoDiscriminator();
    }

    public DbSet<Dynasty> Dynasties => Set<Dynasty>();
    public DbSet<UserInfo> Users => Set<UserInfo>();
}