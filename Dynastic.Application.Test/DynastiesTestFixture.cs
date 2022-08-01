using Dynastic.Application.Common.Interfaces;
using Dynastic.Architecture.Configuration;
using Dynastic.Domain.Common.Interfaces;
using Dynastic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dynastic.Application.Test;

// TODO: Improve with mocking
public class CurrentUserService : ICurrentUserService
{
    public string UserId => "auth0|623eef5eace84900685371a0";
}

public class DynastiesTestFixture : IDisposable, IAsyncLifetime
{
    public ApplicationDbContext context;
    public ICurrentUserService currentUserService = new CurrentUserService();

    public void Dispose()
    {
    }

    public async Task DisposeAsync()
    {
        await context.Database.EnsureDeletedAsync();
    }

    public async Task InitializeAsync()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(@"appsettings.Development.json")
            .Build();
        var cosmosDbConfig = new CosmosDbConfiguration();
        config.Bind("CosmosDb", cosmosDbConfig);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseCosmos(cosmosDbConfig.EndpointUri, cosmosDbConfig.PrimaryKey, "Dynastic")
            .Options;
        context = new ApplicationDbContext(options);

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }
}