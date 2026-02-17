using Blazicons;

namespace SmoothNanners.Web.Models.Portal;

public sealed class PortalCard
{
    public required Variant Variant { get; init; }

    public required SvgIcon HeadingIcon { get; init; }

    public required string HeadingText { get; init; }

    public required IList<PortalLink> Links { get; init; }

    public IReadOnlyList<string> YouTubeVideoIds { get; init; } = [];
}
