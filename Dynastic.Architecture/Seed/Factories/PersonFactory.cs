using Dynastic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Infrastrucutre.Seed.Factories;

public static class PersonFactory
{
    public static Person Generate()
    {
        var father = Faker.RandomNumber.Next(1, 10) > 6 ? Generate() : null;
        var mother = Faker.RandomNumber.Next(1, 10) > 6 ? Generate() : null;
        return new()
        {
            BirthDate = new DateTime(Faker.RandomNumber.Next(1900, 2000), Faker.RandomNumber.Next(1, 12), Faker.RandomNumber.Next(1, 28)),
            Firstname = Faker.Name.First(),
            Lastname = Faker.Name.Last(),
            MiddleName = Faker.Name.Middle(),
            FatherId = father?.Id,
            MotherId = mother?.Id,
        };
    }
}
