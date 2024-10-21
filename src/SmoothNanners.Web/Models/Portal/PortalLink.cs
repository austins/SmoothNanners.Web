using BootstrapIcons.Net;

namespace SmoothNanners.Web.Models.Portal;

public sealed class PortalLink
{
    public required string Url { get; init; }

    public required string Text { get; init; }

    public required BootstrapIconGlyph Icon { get; init; }

    public required BootstrapVariant BackgroundVariant { get; init; }
}
