using Microsoft.AspNetCore.Razor.TagHelpers;
using SmoothNanners.Web.TagHelpers;

namespace SmoothNanners.Web.Components.Portal;

[HtmlTargetElement("youtube-embeds", TagStructure = TagStructure.WithoutEndTag)]
public sealed class YouTubeEmbeds : ComponentTagHelper
{
    public required IReadOnlyList<string> VideoIds { get; set; }
}
