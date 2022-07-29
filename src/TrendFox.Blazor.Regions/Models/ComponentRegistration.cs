namespace TrendFox.Blazor.Regions;

/// <summary>
/// Component registration used by region registry.
/// </summary>
public class ComponentRegistration
{
    /// <summary>
    /// The key used to register this type. Must be unique per type.
    /// </summary>
    public string Key { get; private set; }

    /// <summary>
    /// The type registered.
    /// </summary>
    public Type Type { get; private set; }

    /// <summary>
    /// Parameters to be used when creating the type.
    /// </summary>
    public IDictionary<string, object?>? Parameters { get; private set; }

    /// <summary>
    /// Creates a new region registration with a type and parameters.
    /// </summary>
    /// <param name="key">The key to differentiate between multiple components of the same type.</param>
    /// <param name="type">The type to be registered.</param>
    /// <param name="parameters">Parameters to be registerd for type creation.</param>
    public ComponentRegistration(string key, Type type, IDictionary<string, object?>? parameters)
    {
        Key = key;
        Type = type;
        Parameters = parameters;
    }
}