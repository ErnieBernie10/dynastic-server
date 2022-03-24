using Dynastic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasMany(p => p.FathersChildren)
                .WithOne(p => p.Father)
                .HasForeignKey(p => p.FatherId);
            modelBuilder.Entity<Person>()
                .HasMany(p => p.MothersChildren)
                .WithOne(p => p.Mother)
                .HasForeignKey(p => p.MotherId);
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
            modelBuilder.Entity<Relationship>()
                .HasOne(p => p.Person)
                .WithMany(p => p.Relationships)
                .HasForeignKey(e => e.PersonId);
            modelBuilder.Entity<UserDynasty>()
                .HasKey(ud => new
                {
                    UserId = ud.UserId,
                    DynastyId = ud.DynastyId
                });
        }


        public DbSet<Person> Persons { get; set; }
        public DbSet<Dynasty> Dynasties { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<UserDynasty> UserDynasties { get; set; }
    }
}
