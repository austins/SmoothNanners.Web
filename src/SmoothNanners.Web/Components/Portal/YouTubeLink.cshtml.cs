using Microsoft.AspNetCore.Razor.TagHelpers;
using SmoothNanners.Web.TagHelpers;

namespace SmoothNanners.Web.Components.Portal;

[HtmlTargetElement("youtube-link", TagStructure = TagStructure.WithoutEndTag)]
public sealed class YouTubeLink : ComponentTagHelper
{
    public required string ChannelId { get; set; }
}
