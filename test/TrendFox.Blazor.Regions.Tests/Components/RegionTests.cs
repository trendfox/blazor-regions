using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace TrendFox.Blazor.Regions.Tests;

public sealed class RegionTests
    : IDisposable
{
    private const string RegionName = "Test";
    private readonly TestContext _ctx = new();

    public RegionTests() => _ctx.Services.AddRegions();
    public void Dispose() => _ctx.Dispose();

    private IRenderedComponent<Region> CreateComponentUnderTest(bool useComponentTemplate = false)
    {
        return _ctx.RenderComponent<Region>(parameters =>
        {
            parameters.Add(p => p.Name, RegionName);

            if (useComponentTemplate)
            {
                var rf = new RenderFragment<RenderFragment>(target =>
                {
                    var fragment = new RenderFragment(t =>
                    {
                        t.OpenElement(0, "div");
                        t.AddContent(1, target);
                        t.CloseElement();
                    });
                    return fragment;
                });

                parameters.Add(p => p.ChildContent, rf);
            }
        });
    }

    [Fact]
    public void RendersNoComponentIfNothingRegistered()
    {
        // Arrange
        // Act
        var cut = CreateComponentUnderTest();

        // Assert
        cut.MarkupMatches("");
    }

    [Fact]
    public void RendersNoComponentIfDifferentRegion()
    {
        // Arrange
        var registry = _ctx.Services.GetService<IRegionRegistry>()!;
        registry.Register<TestComponent>("OtherRegion");

        // Act
        var cut = CreateComponentUnderTest();

        // Assert
        cut.MarkupMatches("");
    }

    [Fact]
    public void RendersComponents()
    {
        // Arrange
        var registry = _ctx.Services.GetService<IRegionRegistry>()!;
        registry.Register<TestComponent>(RegionName);

        // Act
        var cut = CreateComponentUnderTest();

        // Assert
        cut.Find("p").MarkupMatches("<p>TestComponent</p>");
    }

    [Fact]
    public void RendersMultipleComponents()
    {
        // Arrange
        var registry = _ctx.Services.GetService<IRegionRegistry>()!;
        registry.Register<TestComponent>(RegionName, "1");
        registry.Register<TestComponent>(RegionName, "2");

        // Act
        var cut = CreateComponentUnderTest();

        // Assert
        cut.FindAll("p").MarkupMatches("<p>TestComponent</p><p>TestComponent</p>");
    }

    [Fact]
    public void RendersComponentTemplate()
    {
        // Arrange
        var registry = _ctx.Services.GetService<IRegionRegistry>()!;
        registry.Register<TestComponent>(RegionName, "1");

        // Act
        var cut = CreateComponentUnderTest(true);

        // Assert
        cut.FindAll("div").MarkupMatches("<div><p>TestComponent</p></div>");
    }

    [Fact]
    public void RendersComponentTemplateWithParameters()
    {
        // Arrange
        var registry = _ctx.Services.GetService<IRegionRegistry>()!;
        registry.Register<TestComponent>(RegionName, "1", new Dictionary<string, object?> { { nameof(TestComponent.Text), "tst" } });

        // Act
        var cut = CreateComponentUnderTest(true);

        // Assert
        cut.FindAll("div").MarkupMatches("<div><p>TestComponent: tst</p></div>");
    }

    [Fact]
    public void DoesNotRenderComponentIfNotAuthorized()
    {
        // Arrange
        var authCtx = _ctx.AddTestAuthorization();
        authCtx.SetNotAuthorized();

        var registry = _ctx.Services.GetService<IRegionRegistry>()!;
        registry.Register<TestComponentAuth>(RegionName);

        // Act
        var cut = CreateComponentUnderTest();

        // Assert
        cut.MarkupMatches("");
    }

    [Fact]
    public void DoesRenderComponentIfAuthorized()
    {
        // Arrange
        var authCtx = _ctx.AddTestAuthorization();
        authCtx.SetAuthorized("");

        var registry = _ctx.Services.GetService<IRegionRegistry>()!;
        registry.Register<TestComponentAuth>(RegionName);

        // Act
        var cut = CreateComponentUnderTest();

        // Assert
        cut.Find("p").MarkupMatches("<p>TestComponentAuth</p>");
    }

    [Fact]
    public void DoesNotRenderComponentIfNotInRole()
    {
        // Arrange
        var authCtx = _ctx.AddTestAuthorization();
        authCtx.SetAuthorized("");

        var registry = _ctx.Services.GetService<IRegionRegistry>()!;
        registry.Register<TestComponentRole>(RegionName);

        // Act
        var cut = CreateComponentUnderTest();

        // Assert
        cut.MarkupMatches("");
    }

    [Fact]
    public void DoesRenderComponentIfInRole()
    {
        // Arrange
        var authCtx = _ctx.AddTestAuthorization();
        authCtx.SetAuthorized("");
        authCtx.SetRoles("Test role");

        var registry = _ctx.Services.GetService<IRegionRegistry>()!;
        registry.Register<TestComponentRole>(RegionName);

        // Act
        var cut = CreateComponentUnderTest();

        // Assert
        cut.Find("p").MarkupMatches("<p>TestComponentRole</p>");
    }

    [Fact]
    public void DoesNotRenderComponentIfNotPolicy()
    {
        // Arrange
        var authCtx = _ctx.AddTestAuthorization();
        authCtx.SetAuthorized("");
        authCtx.SetPolicies();

        var registry = _ctx.Services.GetService<IRegionRegistry>()!;
        registry.Register<TestComponentPolicy>(RegionName);

        // Act
        var cut = CreateComponentUnderTest();

        // Assert
        cut.MarkupMatches("");
    }

    [Fact]
    public void DoesRenderComponentIfPolicy()
    {
        // Arrange
        var authCtx = _ctx.AddTestAuthorization();
        authCtx.SetAuthorized("");
        authCtx.SetPolicies("Test policy");

        var registry = _ctx.Services.GetService<IRegionRegistry>()!;
        registry.Register<TestComponentPolicy>(RegionName);

        // Act
        var cut = CreateComponentUnderTest();

        // Assert
        cut.Find("p").MarkupMatches("<p>TestComponentPolicy</p>");
    }

    [Fact]
    public void RendersComponentWithInjectedService()
    {
        // Arrange
        _ctx.Services.Add(new ServiceDescriptor(typeof(ITestService), typeof(TestService), ServiceLifetime.Scoped));

        var registry = _ctx.Services.GetService<IRegionRegistry>()!;
        registry.Register<TestComponentWithService>(RegionName);

        // Act
        var cut = CreateComponentUnderTest();

        // Assert
        cut.MarkupMatches("<p>Service somponent</p>");
    }

    [Fact]
    public void RegisterComponentAddsRegistration()
    {
        // Arrange
        var expectedValue = "v";
        var registry = _ctx.Services.GetService<IRegionRegistry>()!;

        // Act
        registry.Register<TestComponent>(RegionName, parameters => parameters.Add(p => p.Text, expectedValue));

        // Assert
        var actual = registry.GetRegistrations(RegionName);
        Assert.Contains(
            actual,
            r => r.Type == typeof(TestComponent)
                && r.Key == ""
                && r.Parameters is not null
                && (string?)r.Parameters[nameof(TestComponent.Text)] == expectedValue);
    }

    [Fact]
    public void RegisterComponentWithKeyAddsRegistration()
    {
        // Arrange
        var expectedKey = "k";
        var expectedValue = "v";
        var registry = _ctx.Services.GetService<IRegionRegistry>()!;

        // Act
        registry.Register<TestComponent>(RegionName, expectedKey, parameters => parameters.Add(p => p.Text, expectedValue));

        // Assert
        var actual = registry.GetRegistrations(RegionName);
        Assert.Contains(
            actual,
            r => r.Type == typeof(TestComponent)
                && r.Key == expectedKey
                && r.Parameters is not null
                && (string?)r.Parameters[nameof(TestComponent.Text)] == expectedValue);
    }

    [Fact]
    public void RendersComponentWithParameterBuilder()
    {
        // Arrange
        var registry = _ctx.Services.GetService<IRegionRegistry>()!;
        registry.Register<TestComponent>(RegionName, parameters => parameters.Add(p => p.Text, "x"));

        // Act
        var cut = CreateComponentUnderTest();

        // Assert
        cut.FindAll("p").MarkupMatches("<p>TestComponent: x</p>");
    }

    [Fact]
    public void RendersComponentWithKeyAndParameterBuilder()
    {
        // Arrange
        var registry = _ctx.Services.GetService<IRegionRegistry>()!;
        registry.Register<TestComponent>(RegionName, "", p => p.Add(c => c.Text, "x"));

        // Act
        var cut = CreateComponentUnderTest();

        // Assert
        cut.FindAll("p").MarkupMatches("<p>TestComponent: x</p>");
    }

    [Fact]
    public async Task UnregisterComponentRaiseWithoutRegionNameRemovesComponentFromRegionAsync()
    {
        // Arrange
        var registry = _ctx.Services.GetService<IRegionRegistry>()!;
        registry.Register<TestComponent>(RegionName, "", p => p.Add(c => c.Text, "x"));
        var cut = CreateComponentUnderTest();
        cut.FindAll("p").MarkupMatches("<p>TestComponent: x</p>");

        // Act
        registry.Unregister<TestComponent>(RegionName);
        await cut.InvokeAsync(() => registry.RaiseRegionsChanged());

        // Assert
        cut.MarkupMatches("");
    }

    [Fact]
    public async Task UnregisterComponentRaiseWithRegionNameRemovesComponentFromRegionAsync()
    {
        // Arrange
        var registry = _ctx.Services.GetService<IRegionRegistry>()!;
        registry.Register<TestComponent>(RegionName, "", p => p.Add(c => c.Text, "x"));
        var cut = CreateComponentUnderTest();
        cut.FindAll("p").MarkupMatches("<p>TestComponent: x</p>");

        // Act
        registry.Unregister<TestComponent>(RegionName);
        await cut.InvokeAsync(() => registry.RaiseRegionsChanged(RegionName));

        // Assert
        cut.MarkupMatches("");
    }
}
