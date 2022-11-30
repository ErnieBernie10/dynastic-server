using Azure;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Application.Services;
using Dynastic.Infrastructure.Configuration;
using Dynastic.Infrastructure.Persistence;
using Dynastic.Infrastructure.SearchEntities;
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

    public static async Task<IServiceCollection> AddCloudInfrastructure(this IServiceCollection services,
        CosmosDbConfiguration configuration, CognitiveSearchConfiguration cognitiveSearchConfiguration)
    {
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            options.UseCosmos(configuration.EndpointUri, configuration.PrimaryKey, "Dynastic"));

        // Setup index
        var fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(DynastyIndex));

        var definition = new SearchIndex("dynasty-index", searchFields);

        var adminClient = new SearchIndexClient(
            endpoint: new Uri(cognitiveSearchConfiguration.EndpointUri),
            credential: new AzureKeyCredential(cognitiveSearchConfiguration.QueryKey)
        );

        await adminClient.CreateOrUpdateIndexAsync(definition);

        services.AddAzureClients(azureSearchBuilder => {
            azureSearchBuilder.AddSearchClient(endpoint: new Uri(cognitiveSearchConfiguration.EndpointUri),
                indexName: cognitiveSearchConfiguration.IndexName,
                credential: new AzureKeyCredential(cognitiveSearchConfiguration.QueryKey));
        });

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IDynastySearchContext, DynastySearchContext>();

        return services;
    }
}