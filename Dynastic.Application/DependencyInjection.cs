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

        services.AddScoped<IAccessService, AccessService>();
        services.AddSingleton<ICoaFileService, CoaFileService>();
        services.AddSingleton<IDynastyFilesService, DynastyFileService>();

        services.AddTransient<IUserInfoService, UserInfoService>();

        return services;
    }
}
