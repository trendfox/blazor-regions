namespace TrendFox.Blazor.Regions;

/// <summary>
/// Event arguments for changed regions.
/// </summary>
public class RegionChangedEventArgs
    : EventArgs
{
    /// <summary>
    /// The name of the changed region, or null, if multiple regions changed.
    /// </summary>
    public string[] Regions { get; private set; }

    /// <summary>
    /// Creates new region changed event args for the specified region.
    /// </summary>
    /// <param name="regions">The name of the changed region, or null, if multiple regions.</param>
    public RegionChangedEventArgs(params string[] regions)
    {
        Regions = regions;
    }
}