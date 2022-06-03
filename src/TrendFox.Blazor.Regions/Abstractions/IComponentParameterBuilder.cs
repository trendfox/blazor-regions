using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace TrendFox.Blazor.Regions;

/// <summary>
/// Interface to build parameter key-value pairs for components using strongly typed
/// member expressions.
/// </summary>
/// <typeparam name="TComponent">The component type to provide parameter values for.</typeparam>
public interface IComponentParameterBuilder<TComponent>
    where TComponent : ComponentBase
{
    /// <summary>
    /// Add the specified value for a given property, using a property selector.
    /// </summary>
    /// <typeparam name="TProperty">The property type to select.</typeparam>
    /// <param name="propertySelector">The member expression targeting the desired property.</param>
    /// <param name="value">The value to associate with the property.</param>
    /// <returns>The IComponentParameterBuilder instance.</returns>
    IComponentParameterBuilder<TComponent> Add<TProperty>(Expression<Func<TComponent, TProperty>> propertySelector, TProperty value);

    /// <summary>
    /// Builds the dictionary with all the added component parameters.
    /// </summary>
    /// <returns>The dictionary with the added component parameters.</returns>
    public IDictionary<string, object?> Build();
}
