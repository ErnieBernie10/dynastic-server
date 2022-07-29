using Dynastic.Domain.Common.ValueObjects;
using Dynastic.Domain.Entities;
using Dynastic.Infrastructure.Persistence;
using Dynastic.Infrastrucutre.Seed.Factories;
using Microsoft.EntityFrameworkCore;
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
        if (await context.Dynasties.FirstOrDefaultAsync() is null)
        {
            var dynasties = Enumerable.Range(2, 10).Select((_) => DynastyFactory.Generate());

            context.Dynasties.AddRange(dynasties);
            await context.SaveChangesAsync();
        }
    }
}
