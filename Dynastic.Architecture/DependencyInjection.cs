using Azure;
using Azure.Messaging.ServiceBus;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Application.Services;
using Dynastic.Infrastructure.Configuration;
using Dynastic.Infrastructure.Messaging;
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
        CosmosDbConfiguration configuration, CognitiveSearchConfiguration cognitiveSearchConfiguration, ServiceBusConfiguration serviceBusConfiguration)
    {
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            options.UseCosmos(configuration.EndpointUri, configuration.PrimaryKey, "Dynastic"));

        await AddCognitiveSearch(cognitiveSearchConfiguration);
        
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IDynastySearchContext, DynastySearchContext>();

        services.AddAzureClients(azureClientsBuilder =>
        {
            azureClientsBuilder.AddSearchClient(endpoint: new Uri(cognitiveSearchConfiguration.EndpointUri),
                indexName: cognitiveSearchConfiguration.IndexName,
                credential: new AzureKeyCredential(cognitiveSearchConfiguration.QueryKey));

            azureClientsBuilder.AddServiceBusClient(serviceBusConfiguration.PrimaryConnectionString);
        });

        services.AddSingleton<IServiceBus, ServiceBus>();

        return services;
    }

    private static async Task AddCognitiveSearch(CognitiveSearchConfiguration cognitiveSearchConfiguration)
    {
        var fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(DynastyIndex));

        var definition = new SearchIndex("dynasty-index", searchFields);

        var adminClient = new SearchIndexClient(
            endpoint: new Uri(cognitiveSearchConfiguration.EndpointUri),
            credential: new AzureKeyCredential(cognitiveSearchConfiguration.QueryKey)
        );

        await adminClient.CreateOrUpdateIndexAsync(definition);
    }
}