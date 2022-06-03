using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Authorization;

namespace TrendFox.Blazor.Regions.Performance;

public class SingleAttributeBenchmarks
{
    private static Type ComponentType = typeof(AttributedClass).GetType();
    private static ISingleAttribute<AuthorizeAttribute> _singleAttReflection = new ReflectionSingleAttribute<AuthorizeAttribute>();
    private static ISingleAttribute<AuthorizeAttribute> _singleAttCachedReflection = new CachedReflectionSingleAttribute<AuthorizeAttribute>();

    [Benchmark(Baseline = true)]
    public void GetAttribute_ReflectionSingleAttribute()
    {
        var att = _singleAttReflection.GetAttribute(ComponentType);
    }

    [Benchmark]
    public void GetAttribute_CachedReflectionSingleAttribute()
    {
        var att = _singleAttCachedReflection.GetAttribute(ComponentType);
    }
}

[Authorize]
internal class AttributedClass
{ }