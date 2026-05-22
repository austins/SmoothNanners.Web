using Blazicons;
using TechGems.StaticComponents;

namespace SmoothNanners.Web.Components.Portal;

public sealed class PortalLink : StaticComponent
{
    public PortalLinkVariant Variant { get; set; }

    public SvgIcon Icon { get; set; } = null!;

    public string Href { get; set; } = null!;
}

public enum PortalLinkVariant
{
    Black,
    Fuchsia,
    Indigo,
    Purple,
    Red,
    Sky,
    Slate
}
