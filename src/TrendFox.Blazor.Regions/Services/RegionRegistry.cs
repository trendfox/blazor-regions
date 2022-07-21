using Microsoft.AspNetCore.Components;

namespace TrendFox.Blazor.Regions;

/// <inheritdoc/>
public class RegionRegistry
    : IRegionRegistry
{
    private readonly Dictionary<string, Dictionary<Type, Dictionary<string, ComponentRegistration>>> _regions = new();

    /// <inheritdoc/>
    public event EventHandler<RegionChangedEventArgs>? RegionChanged;


    private Dictionary<Type, Dictionary<string, ComponentRegistration>> GetOrCreateRegionRegistration(
        Type type, string region)
    {
        var hasTypeRegistration = _regions.TryGetValue(region, out var typeRegistration);

        if (!hasTypeRegistration)
        {
            typeRegistration = new()
            {
                { type, new() }
            };
            _regions[region] = typeRegistration;
        }

        return typeRegistration!;
    }

    private Dictionary<string, ComponentRegistration> GetOrCreateKeyRegistration(
        Dictionary<Type, Dictionary<string, ComponentRegistration>> typeRegistration, Type type)
    {
        var hasKeyRegistration = typeRegistration.TryGetValue(type, out var keyRegistration);

        if (!hasKeyRegistration)
        {
            keyRegistration = new();
            typeRegistration[type] = keyRegistration;
        }

        return keyRegistration!;
    }

    /// <inheritdoc/>
    public void Register<TComponent>(string region, string key = "", IDictionary<string, object?>? parameters = null)
        where TComponent : ComponentBase
    {
        var type = typeof(TComponent);
        var typeRegistration = GetOrCreateRegionRegistration(type, region);
        var keyRegistration = GetOrCreateKeyRegistration(typeRegistration, type);

        keyRegistration.Add(key, new ComponentRegistration(key, type, parameters));
    }

    /// <inheritdoc/>
    public void Register<TComponent>(string region, string key, Action<IComponentParameterBuilder<TComponent>> configureParameters)
        where TComponent : ComponentBase
    {
        var parameters = new ComponentParameterBuilder<TComponent>();
        configureParameters.Invoke(parameters);
        Register<TComponent>(region, key, parameters.Build());
    }

    /// <inheritdoc/>
    public void Register<TComponent>(string region, Action<IComponentParameterBuilder<TComponent>> configureParameters)
        where TComponent : ComponentBase
    {
        var parameters = new ComponentParameterBuilder<TComponent>();
        configureParameters.Invoke(parameters);
        Register<TComponent>(region, "", parameters.Build());
    }

    /// <inheritdoc/>
    public IEnumerable<ComponentRegistration> GetRegistrations(string region)
    {
        if (_regions.TryGetValue(region, out var typeRegistration))
        {
            return typeRegistration
                .SelectMany(tr => tr.Value)
                .Select(tr => tr.Value);
        }

        return Array.Empty<ComponentRegistration>();
    }

    /// <inheritdoc/>
    public void RaiseRegionsChanged(params string[] regions)
    {
        RegionChanged?.Invoke(this, new RegionChangedEventArgs(regions));
    }

    /// <inheritdoc/>
    public void Unregister<TComponent>(string region, string key = "")
        where TComponent : ComponentBase
    {
        if (_regions.TryGetValue(region, out var typeRegistration))
        {
            Type type = typeof(TComponent);

            if (typeRegistration.TryGetValue(type, out var keyRegistration))
            {
                keyRegistration.Remove(key);
                return;
            }

            var withKey = key == "" ? "" : $" with key \"{key}\"";
            throw new KeyNotFoundException($"The type {typeof(TComponent).Name} is not registered{withKey} with region \"{region}\".");
        }

        throw new KeyNotFoundException($"The region \"{region}\" does not exist.");
    }

    /// <inheritdoc/>
    public bool TryUnregister<TComponent>(string region, string key = "") where TComponent : ComponentBase
    {
        if (_regions.TryGetValue(region, out var typeRegistration))
        {
            Type type = typeof(TComponent);

            if (typeRegistration.TryGetValue(type, out var keyRegistration))
            {
                keyRegistration.Remove(key);
                return true;
            }
            return false;
        }

        throw new KeyNotFoundException($"The region \"{region}\" does not exist.");
    }

    /// <inheritdoc/>
    public bool TryRegister<TComponent>(string region, string key = "", IDictionary<string, object?>? parameters = null)
        where TComponent : ComponentBase
    {
        var type = typeof(TComponent);
        var typeRegistration = GetOrCreateRegionRegistration(type, region);
        var keyRegistration = GetOrCreateKeyRegistration(typeRegistration, type);

        return keyRegistration.TryAdd(key, new ComponentRegistration(key, type, parameters));
    }

    /// <inheritdoc/>
    public bool TryRegister<TComponent>(string region, string key, Action<IComponentParameterBuilder<TComponent>> configureParameters)
        where TComponent : ComponentBase
    {
        var parameters = new ComponentParameterBuilder<TComponent>();
        configureParameters.Invoke(parameters);
        return TryRegister<TComponent>(region, key, parameters.Build());
    }

    /// <inheritdoc/>
    public bool TryRegister<TComponent>(string region, Action<IComponentParameterBuilder<TComponent>> configureParameters)
        where TComponent : ComponentBase
    {
        var parameters = new ComponentParameterBuilder<TComponent>();
        configureParameters.Invoke(parameters);
        return TryRegister<TComponent>(region, "", parameters.Build());
    }
}
