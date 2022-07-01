# TrendFox.Blazor.Regions
Create modular Blazor user interfaces.

Define regions in Blazor that render registered components.
Every region can render multiple components, and the same region
can be rendered multiple times in different places with different
templates.

## Setup
Add a reference to the NuGet package `TrendFox.Blazor.Regions`,
and register the required services with dependency injection:
```c#
builder.Services.AddRegions(); // using TrendFox.Blazor.Regions;
```

## Basic usage
In your app, define regions by using the `Region` component. Use
unique names to differentiate between regions:
```html+razor
<Region Name="DashboardRegion" />
```

In modules, or really anywhere, get the `IRegionRegistry` interface
via dependency injection, and use it to register a component for
a specific region.

```c#
regionRegistry.Register<MyComponent>("DashboardRegion");
```
If you register or unregister components for a region that has already
been rendered, use `RaiseRegionsChanged` to update the region, or
multiple regions:
```c#
regionRegistry.RaiseRegionsChanged(); // Update all regions
regionRegistry.RaiseRegionsChanged("DashboardRegion"); // Update DashboardRegion
regionRegistry.RaiseRegionsChanged("Region1", "Region2"); // Update Region1, Region2
```
It is by design, that regions only update when explicitly calling
`RaiseRegionsChanged`. You can register/unregister loads of components,
and only update the UI once.

## Security
Regions comply with security attributes. If registered components
have an `Authorized` attribute, the component is rendered wrapped
in an `AuthorizeView`, applying the corresponding roles and policies.

```html+razor
@attribute [Authorize]
You can only see this if you're signed in.
```

## Advanced scenarios
You can use templates for regions, allowing you to change the appearance
of registered componentes for a region in different places. Simply set the
child content for the region.

Make sure to include `@context` where you want the original child
components to appear within your template.

### Templates for children of regions
```html+razor
<Region Name="DashboardRegion">
    <div class="my-class">@context</div>
</Region>
```

```html+razor
<ul>
    <Region Name="DashboardRegion">
        <li>@context</li>
    </Region>
</ul>
```

### Template for empty regions
Indicate to the user, that a region is empty by using
the `NoChildren` template:
template.
```html+razor
<Region Name="DashboardRegion">
    <NoChildren>
        <div>This region is empty.</div>
    </NoChildren>
    <ChildContent>
        <div>@context</div>
    </ChildContent>
</Region>
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
will therefore behave like singletons for each user for both Blazor server
and Blazor WASM. Do not use resource intensive objects as parameters for
regions.
