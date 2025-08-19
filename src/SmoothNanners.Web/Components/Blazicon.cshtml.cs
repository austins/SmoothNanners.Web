using Blazicons;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SmoothNanners.Web.TagHelpers;

namespace SmoothNanners.Web.Components;

[HtmlTargetElement("blazicon", TagStructure = TagStructure.WithoutEndTag)]
public sealed class Blazicon : ComponentTagHelper
{
    public required SvgIcon Icon { get; set; }
}
