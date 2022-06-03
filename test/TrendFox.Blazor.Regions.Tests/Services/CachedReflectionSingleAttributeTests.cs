using Microsoft.AspNetCore.Authorization;

namespace TrendFox.Blazor.Regions.Tests;

public class CachedReflectionSingleAttributeTests
{
    private readonly CachedReflectionSingleAttribute<AuthorizeAttribute> _cache = new();

    [Fact]
    public void GetAttributeWorks()
    {
        // Arrange
        // Act
        var att = _cache.GetAttribute(typeof(TestComponentAuth));
        // Assert
        Assert.NotNull(att);
    }

    [Fact]
    public void GetAttributeWorksInherited()
    {
        // Arrange
        // Act
        var att = _cache.GetAttribute(typeof(TestComponentAuthInherited));
        // Assert
        Assert.NotNull(att);
    }

    [Fact]
    public void GetAttributeCaches()
    {
        // Arrange
        var type = typeof(TestComponentAuth);
        // Act
        _ = _cache.GetAttribute(type);
        // Assert
        Assert.True(_cache.IsCached(type));
    }

    [Fact]
    public void AttributeIsNotCached()
    {
        // Arrange
        var type = typeof(TestComponentAuth);
        // Act
        // Assert
        Assert.False(_cache.IsCached(type));
    }
}
