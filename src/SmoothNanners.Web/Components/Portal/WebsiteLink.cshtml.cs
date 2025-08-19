using SmoothNanners.Web.TagHelpers;

namespace SmoothNanners.Web.Components.Portal;

public sealed class WebsiteLink : ComponentTagHelper
{
    public required string Url { get; set; }
}
