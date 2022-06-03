namespace TrendFox.Blazor.Regions;

/// <summary>
/// Interface for getting a single attribute from a type.
/// </summary>
/// <typeparam name="TAttribute">The attribute type.</typeparam>
internal interface ISingleAttribute<TAttribute>
    where TAttribute : Attribute
{
    /// <summary>
    /// Get a single attribute from the target type.
    /// </summary>
    /// <param name="target">The target type to get the attribute from.</param>
    /// <returns>The attribute of the specified type, or null if not found.</returns>
    TAttribute? GetAttribute(Type target);
}
