using Blazicons;
using SmoothNanners.Web.Models;
using SmoothNanners.Web.TagHelpers;

namespace SmoothNanners.Web.Components.Portal;

public sealed class PortalSection : ComponentTagHelper
{
    public required Variant Variant { get; set; }

    public required string HeadingText { get; set; }

    public required SvgIcon HeadingIcon { get; set; }
}
