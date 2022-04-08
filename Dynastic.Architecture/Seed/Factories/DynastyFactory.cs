using Dynastic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Infrastrucutre.Seed.Factories;

public static class DynastyFactory
{
    public static Dynasty Generate() => new()
    {
        CreatedAt = DateTime.Now,
        ModifiedAt = DateTime.Now,
        Description = Faker.Lorem.Paragraph(),
        Name = Faker.Name.Last(),
        Members = Enumerable.Range(1, 10).Select((_) => PersonFactory.Generate()).ToList(),
    };
}
