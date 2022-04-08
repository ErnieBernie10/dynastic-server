using Dynastic.Domain.Common.ValueObjects;
using Dynastic.Domain.Entities;
using Dynastic.Infrastructure.Persistence;
using Dynastic.Infrastrucutre.Seed.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Infrastrucutre.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        if (!context.Dynasties.Any())
        {
            var dynasties = Enumerable.Range(2, 10).Select((_) => new UserDynasty
            {
                UserId = "auth0|623eef5eace84900685371a0",
                Dynasty = DynastyFactory.Generate(),
            });

            context.UserDynasties.AddRange(dynasties);
            await context.SaveChangesAsync();
        }
    }
}
