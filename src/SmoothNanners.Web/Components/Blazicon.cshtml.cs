using Blazicons;
using SmoothNanners.Web.TagHelpers;

namespace SmoothNanners.Web.Components;

public sealed class Blazicon : ComponentTagHelper
{
    public required SvgIcon Icon { get; set; }
}
