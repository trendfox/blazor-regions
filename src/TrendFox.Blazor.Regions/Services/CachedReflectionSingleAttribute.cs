using System.Reflection;
using System.Runtime.CompilerServices;

namespace TrendFox.Blazor.Regions;

/// <summary>
/// Gets a specific attribute using an internal weak reference table
/// to cache a single attribute for a given type.
/// </summary>
/// <typeparam name="TAttribute">The attribute type.</typeparam>
internal class CachedReflectionSingleAttribute<TAttribute>
    : ISingleAttribute<TAttribute>
    where TAttribute : Attribute
{
    private ConditionalWeakTable<Type, TAttribute?> _cache = new();

    internal bool IsCached(Type type)
    {
        return _cache.TryGetValue(type, out _);
    }

    /// <inheritdoc/>
    public TAttribute? GetAttribute(Type target)
    {
        TAttribute? attribute = null;

        if (false == _cache.TryGetValue(target, out attribute))
        {
            attribute = (TAttribute?)target
                .GetCustomAttribute(typeof(TAttribute), true);

            _cache.Add(target, attribute);
        }

        return attribute;
    }
}
