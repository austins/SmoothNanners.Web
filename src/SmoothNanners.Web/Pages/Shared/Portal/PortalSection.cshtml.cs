using Blazicons;
using SmoothNanners.Web.Models;

namespace SmoothNanners.Web.Pages.Shared.Portal;

public sealed record PortalSection(
    Variant Variant,
    string HeadingText,
    SvgIcon HeadingIcon,
    IReadOnlyList<PortalLink> Links,
    IReadOnlyList<string>? YouTubeVideoIds = null);
