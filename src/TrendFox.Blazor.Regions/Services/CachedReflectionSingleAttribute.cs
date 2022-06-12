using System.Reflection;

namespace TrendFox.Blazor.Regions;

/// <summary>
/// Gets a specific attribute for a type using reflection, and
/// caches the attribute for subsequent queries.
/// </summary>
/// <typeparam name="TAttribute">The attribute type.</typeparam>
internal class CachedReflectionSingleAttribute<TAttribute>
    : ISingleAttribute<TAttribute>
    where TAttribute : Attribute
{
    private Dictionary<Type, TAttribute?> _cache = new();

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
