using BootstrapIcons.Net;
using SmoothNanners.Web.Models;
using SmoothNanners.Web.TagHelpers;

namespace SmoothNanners.Web.Pages.Components.Portal;

public sealed class PortalSection : ComponentTagHelper
{
    public required string HeadingText { get; set; }

    public BootstrapIconGlyph Icon { get; set; }

    public BootstrapVariant Variant { get; set; }
}
