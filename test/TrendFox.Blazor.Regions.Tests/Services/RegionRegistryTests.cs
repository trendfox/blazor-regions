using Microsoft.AspNetCore.Components.Forms;

namespace TrendFox.Blazor.Regions.Tests;

public class RegionRegistryTests
{
    private const string RegionName = "Test";
    private readonly IRegionRegistry _registry = new RegionRegistry();

    private static IEnumerable<object?[]> GetRegistrationsWorksTestData()
    {
        yield return new object?[] { typeof(InputText), null };
        yield return new object?[] { typeof(InputText), new Dictionary<string, object?> { { nameof(InputText.Value), "x" } } };
    }

    [Theory]
    [MemberData(nameof(GetRegistrationsWorksTestData))]
    public void GetRegistrationsWorks(Type expectedType, Dictionary<string, object?>? expectedParameters)
    {
        // Arrange

        // The RegisterComponent method has overloads, so filter by name,
        // then pick the one with three parameters, where the third is of
        // type IDictionary
        var method = typeof(IRegionRegistry)
            .GetMethods()
            .Where(m => m.Name == nameof(IRegionRegistry.Register))
            .Select(m => new { m.Name, Info = m, Parameters = m.GetParameters() })
            .Where(m => m.Parameters.Length == 3 && m.Parameters[2].ParameterType.Name.StartsWith("IDictionary"))
            .Select(m => m.Info)
            .Single();

        method = method.MakeGenericMethod(expectedType);
        method.Invoke(_registry, new object?[] { RegionName, "", expectedParameters });

        // Act
        var actual = _registry
            .GetRegistrations(RegionName)
            .ToArray();

        // Assert
        Assert.Single(actual);
        Assert.Equal(expectedType, actual[0].Type);
        Assert.Equal(expectedParameters, actual[0].Parameters);
    }

    [Fact]
    public void NoRegistrationsWorks()
    {
        // Arrange
        // Act
        // Assert
        var actual = _registry.GetRegistrations(RegionName);
        Assert.Empty(actual);
    }

    [Fact]
    public void RegisterWorksForMultipleTypes()
    {
        // Arrange
        // Act
        _registry.Register<TestComponent>(RegionName);
        _registry.Register<TestOtherComponent>(RegionName);
        // Assert
        var actualRegistrations = _registry.GetRegistrations(RegionName);
        Assert.Contains(actualRegistrations, r => r.Type == typeof(TestComponent));
        Assert.Contains(actualRegistrations, r => r.Type == typeof(TestOtherComponent));
    }

    [Fact]
    public void TryRegisterWorksForMultipleTypes()
    {
        // Arrange
        // Act
        _ = _registry.TryRegister<TestComponent>(RegionName);
        _ = _registry.TryRegister<TestOtherComponent>(RegionName);
        // Assert
        var actualRegistrations = _registry.GetRegistrations(RegionName);
        Assert.Contains(actualRegistrations, r => r.Type == typeof(TestComponent));
        Assert.Contains(actualRegistrations, r => r.Type == typeof(TestOtherComponent));
    }

    [Fact]
    public void RegisterTwiceWithSameKeyThrows()
    {
        // Arrange
        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            // Act
            _registry.Register<InputText>(RegionName);
            _registry.Register<InputText>(RegionName);
        });
    }

    [Fact]
    public void RegisterTwiceWithDifferentKeyWorks()
    {
        // Arrange
        // Act
        _registry.Register<InputText>(RegionName, "key1");
        _registry.Register<InputText>(RegionName, "key2");
        // Assert
    }

    [Fact]
    public void TryRegisterTwiceWithDifferentKeyWorks()
    {
        // Arrange
        // Act
        var actual1 = _registry.TryRegister<InputText>(RegionName, "key1");
        var actual2 = _registry.TryRegister<InputText>(RegionName, "key2");
        // Assert
        var actualRegistrations = _registry.GetRegistrations(RegionName);
        Assert.Contains(actualRegistrations, r => r.Key == "key1");
        Assert.Contains(actualRegistrations, r => r.Key == "key2");
        Assert.True(actual1);
        Assert.True(actual2);
    }

    [Fact]
    public void TryRegisterOnceReturnsTrue()
    {
        // Arrange
        // Act
        var actual = _registry.TryRegister<InputText>(RegionName);
        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void TryRegisterTwiceReturnsFalse()
    {
        // Arrange
        _ = _registry.TryRegister<InputText>(RegionName);
        // Act
        var actual = _registry.TryRegister<InputText>(RegionName);
        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void TryRegisterOnceWithKeyReturnsTrue()
    {
        // Arrange
        // Act
        var actual = _registry.TryRegister<InputText>(RegionName, "key");
        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void TryRegisterTwiceWithKeyReturnsFalse()
    {
        // Arrange
        _ = _registry.TryRegister<InputText>(RegionName, "key");
        // Act
        var actual = _registry.TryRegister<InputText>(RegionName, "key");
        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void TryRegisterOnceWithActionReturnsTrue()
    {
        // Arrange
        // Act
        var actual = _registry.TryRegister<InputText>(RegionName, p => p
            .Add(c => c.Value, ""));
        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void TryRegisterTwiceWithActionReturnsFalse()
    {
        // Arrange
        _ = _registry.TryRegister<InputText>(RegionName, p => p
            .Add(c => c.Value, ""));
        // Act
        var actual = _registry.TryRegister<InputText>(RegionName, p => p
            .Add(c => c.Value, ""));
        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void TryRegisterOnceWithKeyAndDictionaryReturnsTrue()
    {
        // Arrange
        // Act
        var actual = _registry.TryRegister<InputText>(RegionName, "key", new Dictionary<string, object?>());
        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void TryRegisterTwiceWithKeyAndDictionaryReturnsFalse()
    {
        // Arrange
        _ = _registry.TryRegister<InputText>(RegionName, "key", new Dictionary<string, object?>());
        // Act
        var actual = _registry.TryRegister<InputText>(RegionName, "key", new Dictionary<string, object?>());
        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void TryRegisterOnceWithKeyAndActionReturnsTrue()
    {
        // Arrange
        // Act
        var actual = _registry.TryRegister<InputText>(RegionName, "key", p => p
            .Add(c => c.Value, ""));
        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void TryRegisterTwiceWithKeyAndActionReturnsFalse()
    {
        // Arrange
        _ = _registry.TryRegister<InputText>(RegionName, "key", p => p
            .Add(c => c.Value, ""));
        // Act
        var actual = _registry.TryRegister<InputText>(RegionName, "key", p => p
            .Add(c => c.Value, ""));
        // Assert
        Assert.False(actual);
    }

    private static IEnumerable<object?[]> RaiseRegionChangedRaisesEventData()
    {
        yield return new object?[] { null };
        yield return new object?[] { new[] { "1" } };
        yield return new object?[] { new[] { "1", "2", "3" } };
    }

    [Theory]
    [MemberData(nameof(RaiseRegionChangedRaisesEventData))]
    public void RaiseRegionChangedRaisesEvent(string[] expected)
    {
        // Arrange
        // Act
        var receivedEvent = Assert.Raises<RegionChangedEventArgs>(
            handler => _registry.RegionChanged += handler,
            handler => _registry.RegionChanged -= handler,
            () => _registry.RaiseRegionsChanged(expected));
        // Assert
        Assert.NotNull(receivedEvent);
        Assert.Equal(expected, receivedEvent.Arguments.Regions);
    }

    [Fact]
    public void RaiseRegionChangedWithoutSubscriberWorks()
    {
        // Arrange
        // Act
        _registry.RaiseRegionsChanged();
        // Assert
    }

    [Fact]
    public void UnregisterWorks()
    {
        // Arrange
        _registry.Register<TestComponent>(RegionName);

        // Act
        _registry.Unregister<TestComponent>(RegionName);

        // Assert
        var actualRegistrations = _registry.GetRegistrations(RegionName);
        Assert.Empty(actualRegistrations);
    }

    [Fact]
    public void TryUnregisterWorks()
    {
        // Arrange
        _registry.Register<TestComponent>(RegionName);

        // Act
        var actual = _registry.TryUnregister<TestComponent>(RegionName);

        // Assert
        var actualRegistrations = _registry.GetRegistrations(RegionName);
        Assert.Empty(actualRegistrations);
        Assert.True(actual);
    }

    [Fact]
    public void TryUnregisterNotRegisteredWorks()
    {
        // Arrange
        _registry.Register<InputText>(RegionName);
        // Act
        var actual = _registry.TryUnregister<TestComponent>(RegionName);
        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void TryUnregisterWithKeyNotRegisteredWorks()
    {
        // Arrange
        _registry.Register<InputText>(RegionName);
        // Act
        var actual = _registry.TryUnregister<TestComponent>(RegionName, "key");
        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void UnregisterWorksWithKey()
    {
        // Arrange
        var keyExpectDoesNotExist = "1";
        var keyExpectExist = "2";
        _registry.Register<TestComponent>(RegionName, keyExpectDoesNotExist);
        _registry.Register<TestComponent>(RegionName, keyExpectExist);

        // Act
        _registry.Unregister<TestComponent>(RegionName, keyExpectDoesNotExist);

        // Assert
        var actualRegistrations = _registry.GetRegistrations(RegionName);
        Assert.DoesNotContain(actualRegistrations, r => r.Type == typeof(TestComponent) && r.Key == keyExpectDoesNotExist);
        Assert.Contains(actualRegistrations, r => r.Type == typeof(TestComponent) && r.Key == keyExpectExist);
    }


    [Fact]
    public void TryUnregisterWorksWithKey()
    {
        // Arrange
        var keyExpectDoesNotExist = "1";
        var keyExpectExist = "2";
        _registry.Register<TestComponent>(RegionName, keyExpectDoesNotExist);
        _registry.Register<TestComponent>(RegionName, keyExpectExist);

        // Act
        var actual = _registry.TryUnregister<TestComponent>(RegionName, keyExpectDoesNotExist);

        // Assert
        var actualRegistrations = _registry.GetRegistrations(RegionName);
        Assert.DoesNotContain(actualRegistrations, r => r.Type == typeof(TestComponent) && r.Key == keyExpectDoesNotExist);
        Assert.Contains(actualRegistrations, r => r.Type == typeof(TestComponent) && r.Key == keyExpectExist);
        Assert.True(actual);
    }

    [Fact]
    public void UnregisterThrowsWithoutRegion()
    {
        // Arrange
        var expectedMessage = $"The region \"{RegionName}\" does not exist.";

        // Assert
        var actualException = Assert.Throws<KeyNotFoundException>(() =>
        {
            // Act
            _registry.Unregister<TestComponent>(RegionName);
        });
        Assert.Equal(expectedMessage, actualException.Message);

        actualException = Assert.Throws<KeyNotFoundException>(() =>
        {
            // Act
            _registry.Unregister<TestComponent>(RegionName, "1");
        });
        Assert.Equal(expectedMessage, actualException.Message);
    }

    [Fact]
    public void TryUnregisterThrowsWithoutRegion()
    {
        // Arrange
        var expectedMessage = $"The region \"{RegionName}\" does not exist.";

        // Assert
        var actualException = Assert.Throws<KeyNotFoundException>(() =>
        {
            // Act
            _registry.TryUnregister<TestComponent>(RegionName);
        });
        Assert.Equal(expectedMessage, actualException.Message);
    }

    [Fact]
    public void UnregisterThrowsWithoutRegistration()
    {
        // Arrange
        _registry.Register<TestOtherComponent>(RegionName);

        var expectedMessage = $"The type TestComponent is not registered with region \"{RegionName}\".";

        // Assert
        var actualException = Assert.Throws<KeyNotFoundException>(() =>
        {
            // Act
            _registry.Unregister<TestComponent>(RegionName);
        });
        Assert.Equal(expectedMessage, actualException.Message);
    }

    [Fact]
    public void UnregisterWithKeyThrowsWithoutRegistration()
    {
        // Arrange
        var key = "1";
        var registry = (IRegionRegistry)new RegionRegistry();
        registry.Register<TestOtherComponent>(RegionName, key);

        var expectedMessage = $"The type TestComponent is not registered with key \"{key}\" with region \"{RegionName}\".";

        // Assert
        var actualException = Assert.Throws<KeyNotFoundException>(() =>
        {
            // Act
            registry.Unregister<TestComponent>(RegionName, key);
        });
        Assert.Equal(expectedMessage, actualException.Message);
    }
}
