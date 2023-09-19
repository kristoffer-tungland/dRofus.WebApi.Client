using Microsoft.Extensions.DependencyInjection;
using dRofus.WebApi.Client.Services;

namespace dRofus.WebApi.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDRofusServices(this IServiceCollection services, dRofusOptions options)
    {
        services.AddSingleton(options);
        services.AddTransient<dRofusClient>();
        services.AddTransient<IdRofusUserService, dRofusUserService>();
        services.AddTransient<IdRofusOccurrenceService, dRofusOccurrenceService>();
        return services;
    }
}