using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Routes.Pages;
using Index = Routes.Pages.Index;

namespace SmoothNanners.Web.Tests.Integration.Pages;

public sealed class ErrorTests(TestFixture fixture) : TestBase(fixture)
{
    [Theory]
    [InlineData(
        null,
        StatusCodes.Status500InternalServerError,
        "An error occurred while processing your request. Please try again later.")]
    [InlineData(
        StatusCodes.Status404NotFound,
        StatusCodes.Status404NotFound,
        "The resource you are looking for was not found.")]
    public async Task Loads_Successfully_WithNoCache(int? code, int expectedCode, string expectedMessage)
    {
        // Arrange
        var path = GetPath(Error.Get(code));
        var page = await CreatePageAsync();

        // Act
        var response = await page.GotoAsync(path);

        // Assert
        response!.Status.Should().Be(expectedCode);
        (await response.HeaderValueAsync(HeaderNames.CacheControl)).Should().Be("no-store,no-cache");
        (await response.HeaderValueAsync(HeaderNames.Pragma)).Should().Be("no-cache");
        (await response.HeaderValueAsync(HeaderNames.Age)).Should().BeNull();

        var errorHeading = page.Locator("body > div > main h2").First;
        (await errorHeading.TextContentAsync()).Should().Be($"Error: {expectedCode}");

        var errorMessage = errorHeading.Locator("//following-sibling::p[1]");
        (await errorMessage.TextContentAsync()).Should().Be(expectedMessage);

        (await page.Locator("body > div > main a").GetByText("Back to Home").GetAttributeAsync("href"))
            .Should()
            .Be(GetPath(Index.Get()));
    }
}
