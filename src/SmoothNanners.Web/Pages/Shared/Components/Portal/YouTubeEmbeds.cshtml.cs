using Microsoft.AspNetCore.Razor.TagHelpers;
using SmoothNanners.Web.TagHelpers;

namespace SmoothNanners.Web.Pages.Shared.Components.Portal;

[HtmlTargetElement("youtube-embeds", TagStructure = TagStructure.WithoutEndTag)]
public sealed class YouTubeEmbeds : ComponentTagHelper
{
#pragma warning disable CA2227 // Collection properties should be read only
    public required IList<string> VideoIds { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
}
