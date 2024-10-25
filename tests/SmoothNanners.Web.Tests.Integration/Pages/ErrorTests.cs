using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace SmoothNanners.Web.Tests.Integration.Pages;

public sealed class ErrorTests(TestFixture fixture) : TestBase(fixture)
{
    [Theory]
    [InlineData(399, StatusCodes.Status400BadRequest)]
    [InlineData(600, StatusCodes.Status400BadRequest)]
    [InlineData(StatusCodes.Status500InternalServerError, StatusCodes.Status500InternalServerError)]
    [InlineData(
        StatusCodes.Status404NotFound,
        StatusCodes.Status200OK,
        StatusCodes.Status404NotFound,
        "The resource you are looking for was not found.")]
    public async Task Loads_Successfully_WithNoCache(
        int code,
        int expectedResponseCode,
        int? expectedHeadingCode = null,
        string expectedMessage = "An error occurred while processing your request. Please try again later.")
    {
        // Arrange
        var path = GetPath(Routes.Pages.Error.Get(code));
        var page = await CreatePageAsync();

        // Act
        var response = await page.GotoAsync(path);

        // Assert
        response!.Status.Should().Be(expectedResponseCode);
        (await response.HeaderValueAsync(HeaderNames.CacheControl)).Should().Be("no-store,no-cache");
        (await response.HeaderValueAsync(HeaderNames.Pragma)).Should().Be("no-cache");
        (await response.HeaderValueAsync(HeaderNames.Age)).Should().BeNull();

        var errorHeading = page.Locator("body > div > main h2").First;
        (await errorHeading.TextContentAsync()).Should().Be($"Error: {expectedHeadingCode ?? expectedResponseCode}");

        var errorMessage = errorHeading.Locator("//following-sibling::p[1]");
        (await errorMessage.TextContentAsync()).Should().Be(expectedMessage);

        (await page.Locator("body > div > main a").GetByText("Back to Home").GetAttributeAsync("href"))
            .Should()
            .Be(GetPath(Routes.Pages.Index.Get()));
    }
}
