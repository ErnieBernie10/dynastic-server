using Dynastic.Application.Dynasties.Queries;
using Dynastic.Domain.Entities;

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

            var query = new GetDynastiesForUserQueryHandler(AccessService);
            var result = await query.Handle(new GetDynastiesForUserQuery(), CancellationToken.None);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
        
        [Fact]
        async Task GetDynasties_ShouldReturnSomeDynasties_WhenUserIsMemberOfDynasty()
        {
            await Context.AddAsync(new Dynasty {
                Name = "Doe",
                OwnershipProperties = new DynastyOwnershipProperties() { OwnerUserId = "Not the owner", Members = new List<string>() { CurrentUserService.UserId }}
            });
            await Context.SaveChangesAsync();

            var query = new GetDynastiesForUserQueryHandler(AccessService);
            var result = await query.Handle(new GetDynastiesForUserQuery(), CancellationToken.None);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
        
        [Fact]
        async Task GetDynasties_ShouldReturnEmpty_WhenUserHasNoDynasties()
        {
            await Context.AddAsync(new Dynasty {
                Name = "Doe",
                OwnershipProperties = new DynastyOwnershipProperties() { OwnerUserId = "Not the owner", Members = new List<string>() { "Not a member" }}
            });
            await Context.SaveChangesAsync();

            var query = new GetDynastiesForUserQueryHandler(AccessService);
            var result = await query.Handle(new GetDynastiesForUserQuery(), CancellationToken.None);
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}