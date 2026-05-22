using Blazicons;
using TechGems.StaticComponents;

namespace SmoothNanners.Web.Components.Portal;

public sealed class PortalCard : StaticComponent
{
    public PortalCardVariant Variant { get; set; }

    public SvgIcon HeadingIcon { get; set; } = null!;

    public string HeadingText { get; set; } = null!;
}

public enum PortalCardVariant
{
    Indigo,
    Pink,
    Cyan,
    Blue
}
