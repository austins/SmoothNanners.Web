using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using SmoothNanners.Web.Constants;
using SmoothNanners.Web.Tests.Integration.Extensions;

namespace SmoothNanners.Web.Tests.Integration.Controllers;

public sealed class HomeControllerTests : TestBase
{
    [Test]
    public async Task Index_Loads_Successfully_WithLayoutAndNoJavaScriptErrors()
    {
        // Arrange
        var path = GetPath(Routes.Controllers.Home.Index());
        var page = await CreatePageAsync();

        var hasConsoleErrors = false;
        page.Console += (_, message) =>
        {
            if (message.Type == "error")
            {
                hasConsoleErrors = true;
            }
        };

        // Act
        await page.GotoAsync(path);

        // The response output may have been cached previously from another test case, so we reload at least two times to ensure we're seeing a cached page with relevant headers.
        await page.ReloadAsync();
        var cachedResponse = await page.ReloadAsync();

        // Assert
        cachedResponse!.Status.ShouldBe(StatusCodes.Status200OK);
        hasConsoleErrors.ShouldBeFalse();

        (await cachedResponse.HeaderValueAsync(HeaderNames.ContentType)).ShouldBe("text/html; charset=utf-8");
        (await cachedResponse.HeaderValueAsync(HeaderNames.Age)).ShouldNotBeEmpty();

        (await page.Locator("head > title").TextContentAsync()).ShouldBe(AppConstants.SiteName);

        (await page.Locator("head > meta[name='description']").GetAttributeAsync("content")).ShouldBe(
            AppConstants.SiteDescription);

        var siteHeader = page.Locator("body > div > header > h1").First;
        (await siteHeader.TextContentAsync()).ShouldBe(AppConstants.SiteName);
        (await siteHeader.CssValueAsync("font-family")).ShouldContain("Playpen Sans");

        (await page.Locator(".container").First.CssValueAsync("max-width")).ShouldBe("768px");
        (await page.Locator("p").First.CssValueAsync("padding-bottom")).ShouldBe("12px");
    }

    [Test]
    public async Task Index_Click_YouTubeEmbed_NoJavaScript_Success()
    {
        // Arrange
        var path = GetPath(Routes.Controllers.Home.Index());
        var page = await CreatePageAsync(false);

        // Act
        await page.GotoAsync(path);

        var youTubeEmbed = page.Locator("a[href^='https://www.youtube.com/watch?v='][aria-label='YouTube Video']")
            .First;

        var embedContainer = youTubeEmbed.Locator("..").Locator("..");

        await youTubeEmbed.ClickAsync();

        // Assert
        (await embedContainer.Locator("iframe").IsHiddenAsync()).ShouldBeTrue();
    }

    [Test]
    public async Task Index_Click_YouTubeEmbed_WithJavaScript_OnePlayerAtOnce()
    {
        // Arrange
        var path = GetPath(Routes.Controllers.Home.Index());
        var page = await CreatePageAsync();

        // Act
        await page.GotoAsync(path);

        var youtubeEmbedLinks = page.Locator("a[href^='https://www.youtube.com/watch?v='][aria-label='YouTube Video']");
        var embedContainer = youtubeEmbedLinks.First.Locator("..").Locator("..");

        await youtubeEmbedLinks.First.ClickAsync();
        await youtubeEmbedLinks.Last.ClickAsync();

        var iframes = embedContainer.Locator("> iframe");

        // Assert
        (await youtubeEmbedLinks.CountAsync()).ShouldBe(2);
        (await iframes.CountAsync()).ShouldBe(1);
        (await iframes.First.IsVisibleAsync()).ShouldBeTrue();
    }

    [Test]
    [Arguments(399, StatusCodes.Status500InternalServerError)]
    [Arguments(600, StatusCodes.Status500InternalServerError)]
    [Arguments(StatusCodes.Status500InternalServerError, StatusCodes.Status500InternalServerError)]
    [Arguments(
        StatusCodes.Status404NotFound,
        StatusCodes.Status404NotFound,
        "The resource you are looking for was not found.")]
    public async Task Error_Loads_Successfully_WithNoCache(
        int code,
        int expectedResponseCode,
        string expectedMessage = "An error occurred while processing your request. Please try again later.")
    {
        // Arrange
        var path = GetPath(Routes.Controllers.Home.Error(code));
        var page = await CreatePageAsync();

        // Act
        var response = await page.GotoAsync(path);

        // Assert
        response!.Status.ShouldBe(expectedResponseCode);
        (await response.HeaderValueAsync(HeaderNames.CacheControl)).ShouldBe("no-store,no-cache");
        (await response.HeaderValueAsync(HeaderNames.Pragma)).ShouldBe("no-cache");
        (await response.HeaderValueAsync(HeaderNames.Age)).ShouldBeNull();

        (await page.Locator("head > title").TextContentAsync()).ShouldBe(
            $"Error: {expectedResponseCode} | {AppConstants.SiteName}");
        (await page.Locator("head > meta[name='robots']").GetAttributeAsync("content")).ShouldBe("noindex");

        var errorHeading = page.Locator("body > div > main h2").First;
        (await errorHeading.TextContentAsync()).ShouldBe($"Error: {expectedResponseCode}");

        var errorMessage = errorHeading.Locator("//following-sibling::p[1]");
        (await errorMessage.TextContentAsync()).ShouldBe(expectedMessage);

        (await page.Locator("body > div > main a").GetByText("Back to Home").GetAttributeAsync("href")).ShouldBe("/");
    }
}
