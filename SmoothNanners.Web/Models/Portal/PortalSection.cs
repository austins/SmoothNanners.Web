using BootstrapIcons.Net;

namespace SmoothNanners.Web.Models.Portal;

public sealed class PortalSection
{
    public required string HeadingText { get; init; }

    public required BootstrapIconGlyph Icon { get; init; }

    public required ColorVariant BorderVariant { get; init; }

    public IList<PortalLink> Links { get; init; } = [];

    public IList<string> YouTubeVideoIds { get; init; } = [];
}
