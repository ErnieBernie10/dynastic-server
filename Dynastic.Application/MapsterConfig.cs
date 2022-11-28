using Dynastic.Application.Common.Interfaces;
using Dynastic.Application.Dynasties.Queries;
using Dynastic.Application.Services;
using Dynastic.Domain.Entities;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Dynastic.Application;

public static class DynasticMapster
{
    public static void Configure(IServiceProvider serviceCollection)
    {
        var coaFileService = serviceCollection.GetRequiredService<ICoaFileService>();

        TypeAdapterConfig<Dynasty, DynastyBasicDto>.NewConfig()
            .Map(dest => dest.CoaPath, d => coaFileService.GetCoaPath(d));
    }
}