using Dynastic.Application.Common.Interfaces;
using Dynastic.Application.Services;
using Dynastic.Infrastructure.Configuration;
using Dynastic.Infrastructure.Persistence;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInMemoryInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("DynasticDb"));
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        return services;
    }

    public static IServiceCollection AddCloudInfrastructure(this IServiceCollection services,
        CosmosDbConfiguration configuration)
    {
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            options.UseCosmos(configuration.EndpointUri, configuration.PrimaryKey, "Dynastic"));
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IDynasticSearchContext, DynasticSearchContext>();

        return services;
    }
}