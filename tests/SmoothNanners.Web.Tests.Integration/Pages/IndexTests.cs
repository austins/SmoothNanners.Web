using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using SmoothNanners.Web.Constants;
using Index = Routes.Pages.Index;

namespace SmoothNanners.Web.Tests.Integration.Pages;

public sealed class IndexTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task Loads_Successfully_WithLayoutAndNoJavaScriptErrors()
    {
        // Arrange
        var path = GetPath(Index.Get());
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

        // Assert
        response!.Status.Should().Be(StatusCodes.Status200OK);
        hasJsErrors.Should().BeFalse();
        (await response.HeaderValueAsync(HeaderNames.ContentType)).Should().Be("text/html; charset=utf-8");

        (await page.Locator("head > meta[name='description']").GetAttributeAsync("content"))
            .Should()
            .Be(AppConstants.SiteDescription);

        (await page.Locator("body > div > main > h1").First.TextContentAsync()).Should().Be(AppConstants.SiteName);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Click_YouTubeEmbed_Success(bool jsEnabled)
    {
        // Arrange
        var path = GetPath(Index.Get());
        var page = await CreatePageAsync(jsEnabled);

        // Act
        await page.GotoAsync(path);
        var youTubeEmbed = page.Locator("a[href^='https://www.youtube.com/watch?v='][aria-label='YouTube Video']").First;
        var embedContainer = youTubeEmbed.Locator("..").Locator("..");
        await youTubeEmbed.ClickAsync();

        // Assert
        (await embedContainer.Locator("iframe").IsVisibleAsync()).Should().Be(jsEnabled);
    }
}
