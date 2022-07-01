using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace TrendFox.Blazor.Regions;

/// <summary>
/// Defines a region to render registered components.
/// </summary>
public partial class Region
    : ComponentBase, IDisposable // Leave ComponentBase, for Stryker.NET!
{
    [Inject]
    private IRegionRegistry RegionRegistry { get; set; } = null!;
    
    [Inject]
    private ISingleAttribute<AuthorizeAttribute> SingleAttribute { get; set; } = null!;

    /// <summary>
    /// The name of the region used to register components.
    /// </summary>
    [Parameter, EditorRequired]
    public string Name { get; set; } = "";

    /// <summary>
    /// The template used to render registered components.
    /// </summary>
    [Parameter]
    public RenderFragment<RenderFragment>? ChildContent { get; set; }

    /// <summary>
    /// The template used if no child controls are registered for the region.
    /// </summary>
    [Parameter]
    public RenderFragment? NoChildren { get; set; }

    private ComponentRegistration[] Registrations = Array.Empty<ComponentRegistration>();

    ///<inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        LoadRegistrations();
        RegionRegistry.RegionChanged += RegionRegistry_RegionChanged;
    }

    ///<inheritdoc/>
    public void Dispose()
    {
        RegionRegistry.RegionChanged -= RegionRegistry_RegionChanged;
    }

    private void LoadRegistrations()
    {
        Registrations = RegionRegistry
            .GetRegistrations(Name)
            .ToArray();
    }

    private void RegionRegistry_RegionChanged(object? sender, RegionChangedEventArgs e)
    {
        if (e.Regions.Length == 0 || e.Regions.Contains(Name))
        {
            LoadRegistrations();
            StateHasChanged();
        }
    }
}
