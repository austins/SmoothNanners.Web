using Blazicons;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TechGems.StaticComponents;

namespace SmoothNanners.Web.Components.Portal;

public sealed class PortalCard : StaticComponent
{
    /// <summary>
    /// Content that isn't a list of links, rendered after the list.
    /// </summary>
    public const string ExtrasSlot = "extras";

    private const string CurrentItemKey = "PortalCard.Current";

    public SvgIcon HeadingIcon { get; set; } = null!;

    public string HeadingText { get; set; } = null!;

    public PortalCardAccent Accent { get; set; }

    /// <summary>
    /// Heading of the card being rendered. Child components render in partial views, where
    /// <see cref="TagHelperContext.Items"/> doesn't reach, so the card is shared through the request instead.
    /// Cards don't nest, so at most one is current.
    /// </summary>
    /// <param name="viewContext">The view context of the child component.</param>
    /// <returns>The heading text, or <see langword="null"/> outside a card.</returns>
    public static string? GetCurrentHeadingText(ViewContext viewContext)
    {
        return (viewContext.HttpContext.Items[CurrentItemKey] as PortalCard)?.HeadingText;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var items = ViewContext!.HttpContext.Items;
        items[CurrentItemKey] = this;
        await base.ProcessAsync(context, output);
        items.Remove(CurrentItemKey);
    }
}

public enum PortalCardAccent
{
    Tangerine,
    Purple,
    Yellow,
    Cyan
}
