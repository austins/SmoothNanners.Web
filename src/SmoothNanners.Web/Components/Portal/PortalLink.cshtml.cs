using Blazicons;
using TechGems.StaticComponents;

namespace SmoothNanners.Web.Components.Portal;

public sealed class PortalLink : StaticComponent
{
    public SvgIcon Icon { get; set; } = null!;

    public string Href { get; set; } = null!;

    /// <summary>
    /// Appends the enclosing card's heading to the accessible name, to tell apart links that share a visible label.
    /// </summary>
    public bool QualifyName { get; set; }

    public string? CardHeadingText => PortalCard.GetCurrentHeadingText(ViewContext!);
}
