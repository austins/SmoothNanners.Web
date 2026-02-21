using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using SmoothNanners.Web.Constants;
using SmoothNanners.Web.Tests.Integration.Extensions;

namespace SmoothNanners.Web.Tests.Integration.Pages;

public sealed class IndexTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
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
        response!.Status.Should().Be(StatusCodes.Status200OK);
        hasConsoleErrors.Should().BeFalse();

        (await response.HeaderValueAsync(HeaderNames.ContentType)).Should().Be("text/html; charset=utf-8");
        (await response.HeaderValueAsync(HeaderNames.CacheControl)).Should().BeNull();
        (await response.HeaderValueAsync(HeaderNames.Pragma)).Should().BeNull();

        (await page.Locator("head > title").TextContentAsync()).Should().Be(AppConstants.SiteName);

        (await page.Locator("head > meta[name='description']").GetAttributeAsync("content"))
            .Should()
            .Be(AppConstants.SiteDescription);

        var siteHeader = page.Locator("body > div > header > h1").First;
        (await siteHeader.TextContentAsync()).Should().Be(AppConstants.SiteName);
        (await siteHeader.CssValueAsync("font-family")).Should().Contain("Lato");

        (await page.Locator(".container").First.CssValueAsync("max-width")).Should().Be("768px");
        (await page.Locator("p").First.CssValueAsync("margin-bottom")).Should().Be("16px");
        (await page.Locator(".btn-black").First.CssValueAsync("background-color")).Should().Be("rgb(0, 0, 0)");
    }

    [Fact]
    public async Task Index_Click_YouTubeEmbed_NoJavaScript_Success()
    {
        // Arrange
        var path = GetPath(Routes.Pages.Index.Get());
        var page = await CreatePageAsync(false);

        // Act
        await page.GotoAsync(path);

        var youTubeEmbed = page.Locator("a[href^='https://www.youtube.com/watch?v='][aria-label='Play YouTube video']")
            .First;

        var embedContainer = youTubeEmbed.Locator("..").Locator("..");

        await youTubeEmbed.ClickAsync();

        // Assert
        (await embedContainer.Locator("iframe").IsHiddenAsync()).Should().BeTrue();
    }

    [Fact]
    public async Task Index_Click_YouTubeEmbed_WithJavaScript_OnePlayerAtOnce()
    {
        // Arrange
        var path = GetPath(Routes.Pages.Index.Get());
        var page = await CreatePageAsync();

        // Act
        await page.GotoAsync(path);

        var youtubeEmbedLinks =
            page.Locator("a[href^='https://www.youtube.com/watch?v='][aria-label='Play YouTube video']");
        var embedContainer = youtubeEmbedLinks.First.Locator("..").Locator("..");

        await youtubeEmbedLinks.First.ClickAsync();
        await youtubeEmbedLinks.Last.ClickAsync();

        // Assert
        (await youtubeEmbedLinks.CountAsync()).Should().Be(2);

        var iframes = embedContainer.Locator("iframe");
        (await iframes.CountAsync()).Should().Be(1);
        (await iframes.First.IsVisibleAsync()).Should().BeTrue();
    }
}
