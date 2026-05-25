using Blazicons;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TechGems.StaticComponents;

namespace SmoothNanners.Web.Components;

[HtmlTargetElement("icon", TagStructure = TagStructure.WithoutEndTag)]
public sealed class Icon : StaticNode
{
    public SvgIcon SvgIcon { get; set; } = null!;
}
