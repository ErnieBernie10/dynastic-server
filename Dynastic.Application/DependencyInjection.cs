using Dynastic.Application.Common.Interfaces;
using Dynastic.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Dynastic.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddSingleton<IAccessService, AccessService>();
        services.AddSingleton<ICoaFileService, CoaFileService>();
        
        return services;
    }
}
