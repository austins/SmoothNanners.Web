using Blazicons;
using TechGems.StaticComponents;

namespace SmoothNanners.Web.Components.Portal;

public sealed class PortalLink : StaticComponent
{
    public SvgIcon Icon { get; set; } = null!;

    public string Href { get; set; } = null!;
}
