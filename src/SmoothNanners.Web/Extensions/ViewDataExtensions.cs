using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Diagnostics.CodeAnalysis;

namespace SmoothNanners.Web.Extensions;

internal static class ViewDataExtensions
{
    private const string TitleKey = "Title";

    extension(ViewDataDictionary viewData)
    {
        public bool TryGetTitle([NotNullWhen(true)] out string? title)
        {
            if (viewData[TitleKey] is string value && !string.IsNullOrWhiteSpace(value))
            {
                title = value;
                return true;
            }

            title = null;
            return false;
        }

        public string SetTitle(string title)
        {
            viewData[TitleKey] = title;
            return title;
        }
    }
}
