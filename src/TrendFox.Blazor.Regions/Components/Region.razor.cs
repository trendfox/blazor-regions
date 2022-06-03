using Microsoft.AspNetCore.Components;

namespace TrendFox.Blazor.Regions
{
    public partial class Region
    {
        [Parameter]
        public string? Name { get; set; }

        [Parameter]
        public RenderFragment<RenderFragment>? ComponentTemplate { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            RegionRegistry.RegionChanged += RegionRegistry_RegionChanged;
        }

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
