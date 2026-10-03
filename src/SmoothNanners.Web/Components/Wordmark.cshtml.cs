using Microsoft.AspNetCore.Razor.TagHelpers;
using TechGems.StaticComponents;

namespace SmoothNanners.Web.Components;

[HtmlTargetElement("wordmark", TagStructure = TagStructure.WithoutEndTag)]
public sealed class Wordmark : StaticNode
{
    public string Text { get; set; } = null!;
}
