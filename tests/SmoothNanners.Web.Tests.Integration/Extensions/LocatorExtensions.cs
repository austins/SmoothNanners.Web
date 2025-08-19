using Microsoft.Playwright;

namespace SmoothNanners.Web.Tests.Integration.Extensions;

internal static class LocatorExtensions
{
    public static Task<string> CssValueAsync(this ILocator locator, string cssPropName)
    {
        return locator.EvaluateAsync<string>($"element => getComputedStyle(element).getPropertyValue('{cssPropName}')");
    }
}
