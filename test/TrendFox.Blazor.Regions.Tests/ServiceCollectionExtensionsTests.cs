using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace TrendFox.Blazor.Regions.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddRegionsRegistersServices()
    {
        // Arrange
        var services = (IServiceCollection)new ServiceCollection();
        
        // Act
        services.AddRegions();

        // Assert
        Assert.Contains(
            services,
            s => s.ServiceType == typeof(ISingleAttribute<>)
                && s.Lifetime == ServiceLifetime.Singleton);

        Assert.Contains(
            services,
            s => s.ServiceType == typeof(IRegionRegistry)
                && s.Lifetime == ServiceLifetime.Scoped);
    }
}
