using Blazicons;
using SmoothNanners.Web.Models;

namespace SmoothNanners.Web.Pages.Shared.Portal;

public sealed record PortalLink(Variant Variant, SvgIcon Icon, string Text, string Url)
{
    public static PortalLink CreateYouTubeLink(string channelId)
    {
        return new PortalLink(
            Variant.Red,
            BootstrapIcon.Youtube,
            "YouTube",
            $"https://www.youtube.com/channel/{channelId}");
    }

    public static PortalLink CreateWebsiteLink(string text, string url)
    {
        return new PortalLink(Variant.Slate, BootstrapIcon.Globe, text, url);
    }
}
