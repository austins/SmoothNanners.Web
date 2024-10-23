using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SmoothNanners.Web.Extensions;

internal static class ViewDataExtensions
{
    private const string TitleKey = "Title";

    public static string? GetTitle(this ViewDataDictionary viewData)
    {
        return viewData[TitleKey] is string title && !string.IsNullOrWhiteSpace(title) ? title : null;
    }

    public static void SetTitle(this ViewDataDictionary viewData, string title)
    {
        viewData[TitleKey] = title;
    }
}
