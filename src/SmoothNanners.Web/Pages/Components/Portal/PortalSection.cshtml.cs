using BootstrapIcons.Net;
using SmoothNanners.Web.Models;
using SmoothNanners.Web.TagHelpers;

namespace SmoothNanners.Web.Pages.Components.Portal;

public sealed class PortalSection : ComponentTagHelper
{
    public required string HeadingText { get; set; }

    public required BootstrapIconGlyph Icon { get; set; }

    public required BootstrapVariant Variant { get; set; }
}
