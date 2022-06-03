using Microsoft.Extensions.DependencyInjection;

namespace TrendFox.Blazor.Regions;

/// <summary>
/// Dependency injection extensions for regions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add services for region managment to dependency injection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddRegions(this IServiceCollection services)
    {
        services.AddSingleton(typeof(ISingleAttribute<>), typeof(CachedReflectionSingleAttribute<>));
        services.AddScoped<IRegionRegistry, RegionRegistry>();
        return services;
    }
}
