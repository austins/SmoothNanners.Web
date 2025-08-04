using Microsoft.Net.Http.Headers;
using SmoothNanners.Web.Constants;
using System.Net;

namespace SmoothNanners.Web.Tests.Integration.Pages;

public sealed class ErrorTests : TestBase
{
    [Test]
    [Arguments(HttpStatusCode.OK, HttpStatusCode.BadRequest)]
    [Arguments(HttpStatusCode.InternalServerError, HttpStatusCode.InternalServerError)]
    [Arguments(HttpStatusCode.NotFound, HttpStatusCode.NotFound, "The page you are looking for was not found.")]
    public async Task Error_Loads_Successfully_WithNoCache(
        HttpStatusCode code,
        HttpStatusCode expectedResponseCode,
        string expectedMessage = "An error occurred while processing your request. Please try again later.")
    {
        // Arrange
        var path = GetPath(Routes.Pages.Error.Get(code));
        var page = await CreatePageAsync();

        // Act
        var response = await page.GotoAsync(path);

        // Assert
        response!.Status.ShouldBe((int)expectedResponseCode);
        (await response.HeaderValueAsync(HeaderNames.CacheControl)).ShouldBe("no-store,no-cache");
        (await response.HeaderValueAsync(HeaderNames.Pragma)).ShouldBe("no-cache");
        (await response.HeaderValueAsync(HeaderNames.Age)).ShouldBeNull();

        (await page.Locator("head > title").TextContentAsync()).ShouldBe(
            $"Error: {(int)expectedResponseCode} | {AppConstants.SiteName}");
        (await page.Locator("head > meta[name='robots']").GetAttributeAsync("content")).ShouldBe("noindex");

        var errorHeading = page.Locator("body > div > main h2").First;
        (await errorHeading.TextContentAsync()).ShouldBe($"Error: {(int)expectedResponseCode}");

        var errorMessage = errorHeading.Locator("//following-sibling::p[1]");
        (await errorMessage.TextContentAsync()).ShouldBe(expectedMessage);

        (await page.Locator("body > div > main a").GetByText("Back to Home").GetAttributeAsync("href")).ShouldBe("/");
    }
}
