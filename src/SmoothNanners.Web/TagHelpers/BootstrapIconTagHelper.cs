using BootstrapIcons.AspNetCore;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SmoothNanners.Web.TagHelpers;

[HtmlTargetElement("svg", Attributes = "bootstrap-icon")]
public sealed class BootstrapIconTagHelper : BoostrapIconTagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        base.Process(context, output);

        if (!output.Attributes.ContainsName("width"))
        {
            output.Attributes.SetAttribute("width", "1em");
        }

        if (!output.Attributes.ContainsName("height"))
        {
            output.Attributes.SetAttribute("height", "1em");
        }
    }
}
