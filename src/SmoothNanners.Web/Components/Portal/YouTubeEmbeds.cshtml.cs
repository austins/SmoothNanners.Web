using Microsoft.AspNetCore.Razor.TagHelpers;
using TechGems.StaticComponents;

namespace SmoothNanners.Web.Components.Portal;

[HtmlTargetElement("youtube-embeds", TagStructure = TagStructure.WithoutEndTag)]
public sealed class YouTubeEmbeds : StaticNode
{
    public IReadOnlyList<string> VideoIds { get; set; } = [];
}
