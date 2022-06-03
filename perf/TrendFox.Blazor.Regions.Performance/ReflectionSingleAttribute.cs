using System.Reflection;

namespace TrendFox.Blazor.Regions.Performance;

/// <summary>
/// Gets a specific attribute using reflection.
/// </summary>
/// <typeparam name="TAttribute">The attribute type.</typeparam>
internal class ReflectionSingleAttribute<TAttribute>
    : ISingleAttribute<TAttribute>
    where TAttribute : Attribute
{
    /// <inheritdoc/>
    public TAttribute? GetAttribute(Type target)
    {
        var attribute = (TAttribute?)target
            .GetCustomAttribute(typeof(TAttribute), true);

        return attribute;
    }
}
