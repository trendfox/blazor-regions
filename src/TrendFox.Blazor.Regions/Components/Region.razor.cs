using Microsoft.AspNetCore.Components;

namespace TrendFox.Blazor.Regions
{
    /// <summary>
    /// Defines a region to render registered components.
    /// </summary>
    public partial class Region
    {
        /// <summary>
        /// The name of the region used to register components.
        /// </summary>
        [Parameter, EditorRequired]
        public string Name { get; set; } = "";

        /// <summary>
        /// The template used to render registered components.
        /// </summary>
        [Parameter]
        public RenderFragment<RenderFragment>? ComponentTemplate { get; set; }

        ///<inheritdoc/>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            RegionRegistry.RegionChanged += RegionRegistry_RegionChanged;
        }

        ///<inheritdoc/>
        public void Dispose()
        {
            RegionRegistry.RegionChanged -= RegionRegistry_RegionChanged;
        }

        private void RegionRegistry_RegionChanged(object? sender, RegionChangedEventArgs e)
        {
            if (e.Regions.Length == 0 || e.Regions.Contains(Name))
            {
                StateHasChanged();
            }
        }
    }
}
