using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SmoothNanners.Web.Extensions;

internal static class ViewDataExtensions
{
    private const string TitleKey = "Title";
    private const string ScriptsKey = "Scripts";

    public static string? GetTitle(this ViewDataDictionary viewData)
    {
        return viewData[TitleKey] is string title && !string.IsNullOrWhiteSpace(title) ? title : null;
    }

    public static void SetTitle(this ViewDataDictionary viewData, string title)
    {
        viewData[TitleKey] = title;
    }

    public static IReadOnlyDictionary<string, bool> GetScripts(this ViewDataDictionary viewData)
    {
        return GetOrInitializeScripts(viewData);
    }

    public static bool TryAddLocalScript(this ViewDataDictionary viewData, string src, bool defer = false)
    {
        // "~/" is needed to get correct path for static assets.
        if (!src.StartsWith("~/", StringComparison.Ordinal))
        {
            throw new ArgumentException($"Script {nameof(src)} must be a local path.", nameof(src));
        }

        return GetOrInitializeScripts(viewData).TryAdd(src, defer);
    }

    private static OrderedDictionary<string, bool> GetOrInitializeScripts(ViewDataDictionary viewData)
    {
        if (viewData.TryGetValue(ScriptsKey, out var value) && value is OrderedDictionary<string, bool> existingScripts)
        {
            return existingScripts;
        }

        var newScripts = new OrderedDictionary<string, bool>();
        viewData[ScriptsKey] = newScripts;
        return newScripts;
    }
}
