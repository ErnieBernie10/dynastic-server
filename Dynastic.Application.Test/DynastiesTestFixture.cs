using Dynastic.Application.Common.Interfaces;
using Dynastic.Application.Common.Services;
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
    public ApplicationDbContext Context;
    public ICurrentUserService CurrentUserService = new CurrentUserService();
    public IAccessService AccessService;

    public void Dispose()
    {
    }

    public async Task DisposeAsync()
    {
        await Context.Database.EnsureDeletedAsync();
    }

    public async Task InitializeAsync()
    {
        AccessService = new AccessService(CurrentUserService);
        var config = new ConfigurationBuilder()
            .AddJsonFile(@"appsettings.Development.json")
            .Build();
        var cosmosDbConfig = new CosmosDbConfiguration();
        config.Bind("CosmosDb", cosmosDbConfig);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseCosmos(cosmosDbConfig.EndpointUri, cosmosDbConfig.PrimaryKey, "Dynastic")
            .Options;
        Context = new ApplicationDbContext(options);

        await Context.Database.EnsureDeletedAsync();
        await Context.Database.EnsureCreatedAsync();
    }
}