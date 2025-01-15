using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using SmoothNanners.Web.Constants;

namespace SmoothNanners.Web.Tests.Integration.Pages;

public sealed class IndexTests : TestBase
{
    [Test]
    public async Task Loads_Successfully_WithLayoutAndNoJavaScriptErrors()
    {
        // Arrange
        var path = GetPath(Routes.Pages.Index.Get());
        var page = await CreatePageAsync();

        var hasJsErrors = false;
        page.Console += (_, message) =>
        {
            if (message.Type == "error")
            {
                hasJsErrors = true;
            }
        };

        // Act
        var response = await page.GotoAsync(path);

        // The response output may have been cached previously from another test case, so we reload to ensure we're seeing a cached page with relevant headers.
        var cachedResponse = await page.ReloadAsync();

        // Assert
        response!.Status.ShouldBe(StatusCodes.Status200OK);
        hasJsErrors.ShouldBeFalse();
        (await response.HeaderValueAsync(HeaderNames.ContentType)).ShouldBe("text/html; charset=utf-8");

        (await cachedResponse!.HeaderValueAsync(HeaderNames.Age)).ShouldNotBeEmpty();
        (await cachedResponse.HeaderValueAsync(HeaderNames.Date)).ShouldBe(
            await response.HeaderValueAsync(HeaderNames.Date));

        (await page.Locator("head > meta[name='description']").GetAttributeAsync("content")).ShouldBe(
            AppConstants.SiteDescription);

        (await page.Locator("body > div > main > h1").First.TextContentAsync()).ShouldBe(AppConstants.SiteName);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Click_YouTubeEmbed_Success(bool jsEnabled)
    {
        // Arrange
        var path = GetPath(Routes.Pages.Index.Get());
        var page = await CreatePageAsync(jsEnabled);

        // Act
        await page.GotoAsync(path);

        var youTubeVideoLink = page.Locator("a[href^='https://www.youtube.com/watch?v='][aria-label='YouTube Video']")
            .First;

        var embedContainer = youTubeVideoLink.Locator("..").Locator("..");

        await youTubeVideoLink.ClickAsync();

        // Assert
        (await embedContainer.Locator("iframe").IsVisibleAsync()).ShouldBe(jsEnabled);
    }
}
