using Blazicons;
using SmoothNanners.Web.Models;
using SmoothNanners.Web.TagHelpers;

namespace SmoothNanners.Web.Components.Portal;

public sealed class PortalLink : ComponentTagHelper
{
    public required Variant Variant { get; set; }

    public required SvgIcon Icon { get; set; }

    public required string Url { get; set; }
}
