using BootstrapIcons.Net;

namespace SmoothNanners.Web.Models.Home;

public sealed class PortalSection
{
    public required Variant Variant { get; init; }

    public required string HeadingText { get; init; }

    public required BootstrapIconGlyph HeadingIcon { get; init; }

    public IReadOnlyList<PortalLink> PortalLinks { get; init; } = [];

    public IReadOnlyList<string> YouTubeVideoIds { get; init; } = [];
}
