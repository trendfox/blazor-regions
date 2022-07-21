using Microsoft.AspNetCore.Components;

namespace TrendFox.Blazor.Regions;

/// <summary>
/// Service for managing component registration with regions.
/// </summary>
public interface IRegionRegistry
{
    /// <summary>
    /// Raised when the regions changed.
    /// </summary>
    event EventHandler<RegionChangedEventArgs>? RegionChanged;

    /// <summary>
    /// Raise the region changed event.
    /// </summary>
    /// <param name="regions">Array of the region names that changed.</param>
    void RaiseRegionsChanged(params string[] regions);

    /// <summary>
    /// Register a component type with a region.
    /// </summary>
    /// <typeparam name="TComponent">The component type to register.</typeparam>
    /// <param name="region">The region name.</param>
    /// <param name="key">Set a unique key, if the same component type is registered multiple times.</param>
    /// <param name="parameters">The parameters used when creating the component.</param>
    void Register<TComponent>(string region, string key = "", IDictionary<string, object?>? parameters = null)
        where TComponent : ComponentBase;

    /// <summary>
    /// Register a component type with a region.
    /// </summary>
    /// <typeparam name="TComponent">The component type to register.</typeparam>
    /// <param name="region">The region name.</param>
    /// <param name="key">Set a unique key, if the same component type is registered multiple times.</param>
    /// <param name="configureParameters">The configuration lambda to add parameter values.</param>
    void Register<TComponent>(string region, string key, Action<IComponentParameterBuilder<TComponent>> configureParameters)
        where TComponent : ComponentBase;

    /// <summary>
    /// Register a component type with a region.
    /// </summary>
    /// <typeparam name="TComponent">The component type to register.</typeparam>
    /// <param name="region">The region name.</param>
    /// <param name="configureParameters">The configuration lambda to add parameter values.</param>
    void Register<TComponent>(string region, Action<IComponentParameterBuilder<TComponent>> configureParameters)
        where TComponent : ComponentBase;

    /// <summary>
    /// Attempt to register a component type with a region. Returns true,
    /// if the region was newly registered. Returns false, if the component
    /// was registered already.
    /// </summary>
    /// <typeparam name="TComponent">The component type to register.</typeparam>
    /// <param name="region">The region name.</param>
    /// <param name="key">Set a unique key, if the same component type is registered multiple times.</param>
    /// <param name="parameters">The parameters used when creating the component.</param>
    /// <returns>True, if the component was newly registered.</returns>
    bool TryRegister<TComponent>(string region, string key = "", IDictionary<string, object?>? parameters = null)
        where TComponent : ComponentBase;

    /// <summary>
    /// Attempt to register a component type with a region. Returns true,
    /// if the region was newly registered. Returns false, if the component
    /// was registered already.
    /// </summary>
    /// <typeparam name="TComponent">The component type to register.</typeparam>
    /// <param name="region">The region name.</param>
    /// <param name="key">Set a unique key, if the same component type is registered multiple times.</param>
    /// <param name="configureParameters">The configuration lambda to add parameter values.</param>
    /// <returns>True, if the component was newly registered.</returns>
    bool TryRegister<TComponent>(string region, string key, Action<IComponentParameterBuilder<TComponent>> configureParameters)
        where TComponent : ComponentBase;

    /// <summary>
    /// Attempt to register a component type with a region. Returns true,
    /// if the region was newly registered. Returns false, if the component
    /// was registered already.
    /// </summary>
    /// <typeparam name="TComponent">The component type to register.</typeparam>
    /// <param name="region">The region name.</param>
    /// <param name="configureParameters">The configuration lambda to add parameter values.</param>
    /// <returns>True, if the component was newly registered.</returns>
    bool TryRegister<TComponent>(string region, Action<IComponentParameterBuilder<TComponent>> configureParameters)
        where TComponent : ComponentBase;

    /// <summary>
    /// Remove a component registration from a region.
    /// </summary>
    /// <typeparam name="TComponent">The component type to unregister.</typeparam>
    /// <param name="region">The region to remove the component registration from.</param>
    /// <param name="key">The unique key to remove, if the same component type is registered multiple times.</param>
    void Unregister<TComponent>(string region, string key = "")
        where TComponent : ComponentBase;

    /// <summary>
    /// Attempts to remove a component registration from a region. Returns true,
    /// if the component was unregistered. Returns false, if the component was
    /// not registered in the first place.
    /// </summary>
    /// <typeparam name="TComponent">The component type to unregister.</typeparam>
    /// <param name="region">The region to remove the component registration from.</param>
    /// <param name="key">The unique key to remove, if the same component type is registered multiple times.</param>
    /// <returns>True, if the component was unregistered.</returns>
    bool TryUnregister<TComponent>(string region, string key = "")
        where TComponent : ComponentBase;

    /// <summary>
    /// Get all component registraions for the specified region.
    /// </summary>
    /// <param name="region">The region name.</param>
    /// <returns>Returns an enumeration of component registartions.</returns>
    IEnumerable<ComponentRegistration> GetRegistrations(string region);
}
