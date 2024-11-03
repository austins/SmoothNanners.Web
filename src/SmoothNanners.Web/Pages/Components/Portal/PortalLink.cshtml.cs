using BootstrapIcons.Net;
using SmoothNanners.Web.Models;
using SmoothNanners.Web.TagHelpers;

namespace SmoothNanners.Web.Pages.Components.Portal;

public sealed class PortalLink : ComponentTagHelper
{
    public required string Url { get; set; }

    public required string Text { get; set; }

    public required BootstrapIconGlyph Icon { get; set; }

    public required BootstrapVariant Variant { get; set; }
}
