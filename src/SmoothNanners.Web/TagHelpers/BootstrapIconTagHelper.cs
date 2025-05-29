using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SmoothNanners.Web.TagHelpers;

/// <summary>
/// Tag helper that adds additional attributes to <see cref="BootstrapIcons.AspNetCore.BoostrapIconTagHelper" />.
/// It sets a default width and height to the element if none are set prior.
/// </summary>
[HtmlTargetElement("svg", Attributes = "bootstrap-icon")]
public sealed class BootstrapIconTagHelper : BootstrapIcons.AspNetCore.BoostrapIconTagHelper
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
