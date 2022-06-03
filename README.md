# TrendFox.Blazor.Regions
Define regions in Blazor that render registered components.
Every region can render multiple components and the same region
can be rendered multiple times with different templates.

## Setup
Reference the package, and register services with dependency injection:
```c#
builder.Services.AddRegions();
```

## Basic usage
In your app, define regions using the `Region` component
and unique names:
```c#
<Region Name="DashboardRegion" />
```

Via dependency injection, get the `IRegionRegistry` interface, and
use it to register your component for a specific region.

```c#
regionRegistry.Register<MyComponent>("DashboardRegion");
```

## Security
Regions comply with security attributes. If registered components
have an `Authorized` attribute, the component is rendered wrapped
with an `AuthorizeView`, applying the corresponding roles and policies.

```c#
@attribute [Authorize]
You can only see this if you're signed in.
```

```c#
@attribute [Authorize(Roles = "admin, superuser")]
<p>You can only see this if you're in the 'admin' or 'superuser' role.</p>
```

```c#
@attribute [Authorize(Policy = "content-editor")]
<p>You can only see this if you satisfy the 'content-editor' policy.</p>
```

## Advanced scenarios
You can use templates for regions, allowing you to change the appearance
of registered componentes for a region in different places. Simply set the
child content for the region.

Make sure to include `@context` where you want the original child
components to appear within your template.

### Templates for children of regions
```c#
<Region Name="DashboardRegion">
    <div class="my-class">@context</div>
</Region>
```

```c#
<ul>
    <Region Name="DashboardRegion">
        <li>@context</li>
    </Region>
</ul>
```

### Parameters for child components

Registered components can also have parameters, either as
a `IDictionary<string, object?>`

```c#
var componentParameters = new Dictionary<string, object?>
{
    { nameof(MyComponent.PropertyName), "MyValue" }
}

regionRegistry.Register<MyComponent>("DashboardRegion", componentParameters);
```

or using a component parameter builder `IComponentParameterBuilder<TComponent>` action:
```c#
regionRegistry.Register<MyComponent>("DashboardRegion", paramBuilder = paramBuilder
    .Add(c => c.FirstProperty, "FirstValue")
    .Add(c => c.SecondProperty, "SecondValue")
    );
```

**Warning**: The region registry is a `Scoped` service, registered parameters
will therefore behave like singletons for each user. Do not use resource intensive
objects as parameters for regions.
