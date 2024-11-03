using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Reflection;
using TechGems.RazorComponentTagHelpers;

namespace SmoothNanners.Web.TagHelpers;

public abstract class ComponentTagHelper : RazorComponentTagHelper
{
    private readonly Type _instanceType;

    protected ComponentTagHelper()
    {
        _instanceType = GetType();
    }

    /// <summary>
    /// Processes and initializes an instance of the tag helper.
    /// </summary>
    /// <param name="context">Tag helper context.</param>
    /// <param name="output">Tag helper output.</param>
    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        EnsureValidPropsOrThrow(output.Attributes);

        return RenderPartialView(
            $"{_instanceType.Namespace![_instanceType.Assembly.GetName().Name!.Length..].Replace(".", "/", StringComparison.Ordinal)}/{_instanceType.Name}.cshtml",
            output);
    }

    private void EnsureValidPropsOrThrow(TagHelperAttributeList attributes)
    {
        // First, check for non-property attributes as it doesn't require reflection.
        if (attributes.Count > 0)
        {
            throw new InvalidOperationException($"Invalid attributes or props for '{_instanceType!.Name}' component.");
        }

        // Check if any bound non-nullable properties aren't set as they are required.
        var nullabilityInfoContext = new NullabilityInfoContext();

        var missingRequiredProps = _instanceType!
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(
                x => x.GetSetMethod() is not null
                     && nullabilityInfoContext.Create(x).WriteState is NullabilityState.NotNull
                     && !Attribute.IsDefined(x, typeof(HtmlAttributeNotBoundAttribute), false)
                     && x.GetValue(this) is null)
            .Select(x => $"'{x.Name}'")
            .ToList();

        if (missingRequiredProps.Count > 0)
        {
            throw new InvalidOperationException(
                $"Missing required props for '{_instanceType.Name}' component: {string.Join(", ", missingRequiredProps)}.");
        }
    }
}
