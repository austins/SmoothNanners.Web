using BootstrapIcons.Net;

namespace SmoothNanners.Web.Models.Portal;

public sealed class PortalLink
{
    public required Variant Variant { get; init; }

    public required BootstrapIconGlyph Icon { get; init; }

    public required Uri Url { get; init; }

    public required string Text { get; init; }

    public static PortalLink CreateYouTubeLink(string channelId)
    {
        return new PortalLink
        {
            Variant = Variant.Red,
            Icon = BootstrapIconGlyph.Youtube,
            Url = new Uri($"https://www.youtube.com/channel/{channelId}"),
            Text = "YouTube"
        };
    }

    public static PortalLink CreateWebsiteLink(Uri url, string text)
    {
        return new PortalLink
        {
            Variant = Variant.Slate,
            Icon = BootstrapIconGlyph.Globe,
            Url = url,
            Text = text
        };
    }
}
