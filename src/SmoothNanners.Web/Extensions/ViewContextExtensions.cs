using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmoothNanners.Web.Extensions;

internal static class ViewContextExtensions
{
    public static IReadOnlyDictionary<string, bool> GetScripts(this ViewContext viewContext)
    {
        return GetOrInitializeScripts(viewContext);
    }

    public static bool TryAddScript(this ViewContext viewContext, string path)
    {
        var isLocalScript = path.StartsWith('/') || path.StartsWith("~/", StringComparison.Ordinal);
        return GetOrInitializeScripts(viewContext).TryAdd(path, isLocalScript);
    }

    private static Dictionary<string, bool> GetOrInitializeScripts(ViewContext viewContext)
    {
        const string scriptsKey = "ViewScripts";
        viewContext.HttpContext.Items.TryAdd(scriptsKey, new Dictionary<string, bool>());
        return (viewContext.HttpContext.Items[scriptsKey] as Dictionary<string, bool>)!;
    }
}
