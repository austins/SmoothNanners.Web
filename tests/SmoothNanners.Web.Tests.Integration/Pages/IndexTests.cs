using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using SmoothNanners.Web.Constants;

namespace SmoothNanners.Web.Tests.Integration.Pages;

public sealed class IndexTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
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

        // The response output may have been cached previously from another test case, but we reload to make sure that it gets cached.
        var cachedResponse = await page.ReloadAsync();

        // Assert
        response!.Status.Should().Be(StatusCodes.Status200OK);
        hasJsErrors.Should().BeFalse();
        (await response.HeaderValueAsync(HeaderNames.ContentType)).Should().Be("text/html; charset=utf-8");

        (await cachedResponse!.HeaderValueAsync(HeaderNames.Age)).Should().NotBeEmpty();
        (await cachedResponse.HeaderValueAsync(HeaderNames.Date))
            .Should()
            .Be(await response.HeaderValueAsync(HeaderNames.Date));

        (await page.Locator("head > meta[name='description']").GetAttributeAsync("content"))
            .Should()
            .Be(AppConstants.SiteDescription);

        (await page.Locator("body > div > main > h1").First.TextContentAsync()).Should().Be(AppConstants.SiteName);
    }

    [Theory]
    [InlineData(false)]
    /* [InlineData(true)]
     * TODO: Disabled for now mapping of static assets in WebApplicationFactory is figured out. */
    public async Task Click_YouTubeEmbed_Success(bool jsEnabled)
    {
        // Arrange
        var path = GetPath(Routes.Pages.Index.Get());
        var page = await CreatePageAsync(jsEnabled);

        // Act
        await page.GotoAsync(path);

        var youTubeVideoLink = page.Locator("a[href^='https://www.youtube.com/watch?v='][aria-label='YouTube Video']")
            .First;

        var embedContainer = youTubeVideoLink.Locator("..");

        await youTubeVideoLink.ClickAsync();

        // Assert
        (await embedContainer.Locator("iframe").IsVisibleAsync()).Should().Be(jsEnabled);
    }
}
