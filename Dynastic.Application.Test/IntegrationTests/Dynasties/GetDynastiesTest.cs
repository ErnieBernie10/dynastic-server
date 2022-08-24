using Dynastic.Application.Dynasties.Queries;
using Dynastic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dynastic.Application.Test.IntegrationTests.Dynasties
{
    public class GetDynastiesTest : DynastiesTestFixture
    {
        [Fact]
        async Task GetDynasties_ShouldReturnSomeDynasties_WhenUserHasDynasties()
        {
            await Context.AddAsync(new Dynasty {
                Name = "Doe",
                OwnershipProperties = new DynastyOwnershipProperties() { OwnerUserId = CurrentUserService.UserId }
            });
            await Context.SaveChangesAsync();

            var query = new GetDynastiesForUserQueryHandler(Context, AccessService);
            var result = await query.Handle(new GetDynastiesForUserQuery(), CancellationToken.None);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}