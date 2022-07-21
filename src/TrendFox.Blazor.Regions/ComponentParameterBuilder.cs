using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace TrendFox.Blazor.Regions;

internal class ComponentParameterBuilder<TComponent>
    : IComponentParameterBuilder<TComponent>
    where TComponent : ComponentBase
{
    private readonly IDictionary<string, object?> _parameters = new Dictionary<string, object?>();

    public IComponentParameterBuilder<TComponent> Add<TProperty>(Expression<Func<TComponent, TProperty>> propertySelector, TProperty value)
    {
        if (propertySelector is null)
        {
            throw new ArgumentNullException(nameof(propertySelector));
        }

        var memberExpr = propertySelector.Body as MemberExpression;
        var memberName = memberExpr?.Member.Name ?? throw new InvalidOperationException("Could not identify member name from lambda.");
        var componentType = typeof(TComponent);

        if (memberExpr.Member.ReflectedType != componentType
            && componentType.IsAssignableTo(memberExpr.Member.ReflectedType) == false)
        {
            throw new InvalidOperationException($"{nameof(propertySelector)} lambda must be an expression targetting a single property of {componentType.Name}.");
        }

        _parameters.Add(memberName, value);

        return this;
    }

    public IDictionary<string, object?> Build()
    {
        return _parameters;
    }
}
