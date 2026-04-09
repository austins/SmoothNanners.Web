using Blazicons;

namespace SmoothNanners.Web.Models.Portal;

public sealed class PortalLink
{
    public required Variant Variant { get; init; }

    public required SvgIcon Icon { get; init; }

    public required string Text { get; init; }

    public required Uri Url { get; init; }

    public static PortalLink WebsiteLink(string text, Uri url)
    {
        return new PortalLink
        {
            Variant = Variant.Dark,
            Icon = BootstrapIcon.Globe,
            Text = text,
            Url = url
        };
    }

    public static PortalLink YouTubeLink(string channelId)
    {
        return new PortalLink
        {
            Variant = Variant.Danger,
            Icon = BootstrapIcon.Youtube,
            Text = "YouTube",
            Url = new Uri($"https://www.youtube.com/channel/{channelId}")
        };
    }
}
