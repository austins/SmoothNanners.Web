using Blazicons;
using TechGems.StaticComponents;

namespace SmoothNanners.Web.Components.Portal;

public sealed class PortalCard : StaticComponent
{
    public SvgIcon HeadingIcon { get; set; } = null!;

    public string HeadingText { get; set; } = null!;

    public PortalCardAccent Accent { get; set; }
}

public enum PortalCardAccent
{
    Tangerine,
    Purple,
    Yellow,
    Cyan
}
