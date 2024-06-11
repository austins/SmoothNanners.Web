using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Routes.Pages;
using Index = Routes.Pages.Index;

namespace SmoothNanners.Web.Tests.E2E.Pages;

public sealed class ErrorTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task Loads_Successfully_WithNoCache()
    {
        // Arrange
        var path = GetPath(Error.Get());
        var page = await CreatePageAsync();

        // Act
        var response = await page.GotoAsync(path);

        // Assert
        response!.Status.Should().Be(StatusCodes.Status200OK);
        (await response.HeaderValueAsync(HeaderNames.CacheControl)).Should().Be("no-store,no-cache");
        (await response.HeaderValueAsync(HeaderNames.Pragma)).Should().Be("no-cache");
        (await response.HeaderValueAsync(HeaderNames.Age)).Should().BeNull();

        var errorHeading = page.Locator("body > div > main h2").First;
        (await errorHeading.TextContentAsync()).Should().Be("Error: 500");

        var errorMessage = errorHeading.Locator("//following-sibling::p[1]");
        (await errorMessage.TextContentAsync())
            .Should()
            .Be("An error occurred while processing your request. Please try again later.");

        /*var requestId = errorMessage.Locator("//following-sibling::p[1]");
        (await requestId.Locator("strong").TextContentAsync()).Should().Be("Request ID:");
        (await requestId.Locator("code").TextContentAsync()).Should().NotBeEmpty();*/

        (await page.Locator("body > div > main a").GetByText("Back to Home").GetAttributeAsync("href"))
            .Should()
            .Be(GetPath(Index.Get()));
    }
}
