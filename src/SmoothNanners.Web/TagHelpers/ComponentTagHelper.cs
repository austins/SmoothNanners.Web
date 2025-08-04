using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Concurrent;
using System.Reflection;
using TechGems.RazorComponentTagHelpers;

namespace SmoothNanners.Web.TagHelpers;

public abstract class ComponentTagHelper : RazorComponentTagHelper
{
    private static readonly ConcurrentDictionary<Type, IReadOnlyList<PropertyInfo>> ComponentProps = new();
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
    public sealed override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        ValidatePropsOrThrow(output.Attributes);

        return RenderPartialView(
            $"{_instanceType.Namespace![_instanceType.Assembly.GetName().Name!.Length..].Replace('.', '/')}/{_instanceType.Name}.cshtml",
            output);
    }

    public sealed override void Process(TagHelperContext context, TagHelperOutput output)
    {
        base.Process(context, output);
    }

    private void ValidatePropsOrThrow(TagHelperAttributeList attributes)
    {
        // Check for non-property attributes as it doesn't require reflection.
        if (attributes.Count > 0)
        {
            throw new InvalidOperationException(
                $"Invalid attributes or props for '{_instanceType.Name}' component: {string.Join(", ", attributes.Select(x => $"'{x.Name}'"))}.");
        }

        var requiredProps = ComponentProps.GetOrAdd(
            _instanceType,
            type =>
            {
                // Check if any bound non-nullable properties aren't set as they are required.
                var nullabilityInfoContext = new NullabilityInfoContext();

                return type
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x.GetSetMethod() is not null
                                && nullabilityInfoContext.Create(x).WriteState is NullabilityState.NotNull
                                && !Attribute.IsDefined(x, typeof(HtmlAttributeNotBoundAttribute), false))
                    .ToList();
            });

        var missingRequiredProps =
            requiredProps.Where(x => x.GetValue(this) is null).Select(x => $"'{x.Name}'").ToList();

        if (missingRequiredProps.Count > 0)
        {
            throw new InvalidOperationException(
                $"Missing required props for '{_instanceType.Name}' component: {string.Join(", ", missingRequiredProps)}.");
        }
    }
}
