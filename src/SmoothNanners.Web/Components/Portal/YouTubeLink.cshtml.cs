using Microsoft.AspNetCore.Razor.TagHelpers;
using TechGems.StaticComponents;

namespace SmoothNanners.Web.Components.Portal;

[HtmlTargetElement("youtube-link", TagStructure = TagStructure.WithoutEndTag)]
public sealed class YouTubeLink : StaticNode
{
    public string ChannelId { get; set; } = null!;
}
