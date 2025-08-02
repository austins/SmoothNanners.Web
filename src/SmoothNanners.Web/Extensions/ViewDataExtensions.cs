using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Diagnostics.CodeAnalysis;

namespace SmoothNanners.Web.Extensions;

internal static class ViewDataExtensions
{
    private const string TitleKey = "Title";

    public static bool TryGetTitle(this ViewDataDictionary viewData, [NotNullWhen(true)] out string? title)
    {
        if (viewData[TitleKey] is string titleValue && !string.IsNullOrWhiteSpace(titleValue))
        {
            title = titleValue;
            return true;
        }

        title = null;
        return false;
    }

    public static string SetTitle(this ViewDataDictionary viewData, string title)
    {
        viewData[TitleKey] = title;
        return title;
    }
}
