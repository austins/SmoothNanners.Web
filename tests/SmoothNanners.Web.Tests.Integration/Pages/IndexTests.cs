using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using SmoothNanners.Web.Constants;
using SmoothNanners.Web.Tests.Integration.Extensions;

namespace SmoothNanners.Web.Tests.Integration.Pages;

public sealed class IndexTests : TestBase
{
    [Test]
    public async Task Index_Loads_Successfully_WithLayoutAndNoJavaScriptErrors()
    {
        // Arrange
        var path = GetPath(Routes.Pages.Index.Get());
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
        var response = await page.GotoAsync(path);

        // Assert
        response!.Status.ShouldBe(StatusCodes.Status200OK);
        hasConsoleErrors.ShouldBeFalse();

        (await response.HeaderValueAsync(HeaderNames.ContentType)).ShouldBe("text/html; charset=utf-8");
        (await response.HeaderValueAsync(HeaderNames.CacheControl)).ShouldBeNull();
        (await response.HeaderValueAsync(HeaderNames.Pragma)).ShouldBeNull();

        (await page.Locator("head > title").TextContentAsync()).ShouldBe(AppConstants.SiteName);

        (await page.Locator("head > meta[name='description']").GetAttributeAsync("content")).ShouldBe(
            AppConstants.SiteDescription);

        var siteHeader = page.Locator("body > div > header > h1").First;
        (await siteHeader.TextContentAsync()).ShouldBe(AppConstants.SiteName);
        (await siteHeader.CssValueAsync("font-family")).ShouldContain("Lato");

        (await page.Locator(".container").First.CssValueAsync("max-width")).ShouldBe("768px");
        (await page.Locator("p").First.CssValueAsync("margin-bottom")).ShouldBe("16px");
    }

    [Test]
    public async Task Index_Click_YouTubeEmbed_NoJavaScript_Success()
    {
        // Arrange
        var path = GetPath(Routes.Pages.Index.Get());
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
        var path = GetPath(Routes.Pages.Index.Get());
        var page = await CreatePageAsync();

        // Act
        await page.GotoAsync(path);

        var youtubeEmbedLinks = page.Locator("a[href^='https://www.youtube.com/watch?v='][aria-label='YouTube Video']");
        var embedContainer = youtubeEmbedLinks.First.Locator("..").Locator("..");

        await youtubeEmbedLinks.First.ClickAsync();
        await youtubeEmbedLinks.Last.ClickAsync();

        // Assert
        (await youtubeEmbedLinks.CountAsync()).ShouldBe(2);

        var iframes = embedContainer.Locator("iframe");
        (await iframes.CountAsync()).ShouldBe(1);
        (await iframes.First.IsVisibleAsync()).ShouldBeTrue();
    }
}
