﻿using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace SmoothNanners.Web.Tests.Integration.Pages;

public sealed class ErrorTests : TestBase
{
    [Test]
    [Arguments(399, StatusCodes.Status500InternalServerError)]
    [Arguments(600, StatusCodes.Status500InternalServerError)]
    [Arguments(StatusCodes.Status500InternalServerError, StatusCodes.Status500InternalServerError)]
    [Arguments(
        StatusCodes.Status404NotFound,
        StatusCodes.Status404NotFound,
        "The resource you are looking for was not found.")]
    public async Task Loads_Successfully_WithNoCache(
        int code,
        int expectedResponseCode,
        string expectedMessage = "An error occurred while processing your request. Please try again later.")
    {
        // Arrange
        var path = $"/error?code={code}";
        var page = await CreatePageAsync();

        // Act
        var response = await page.GotoAsync(path);

        // Assert
        response!.Status.ShouldBe(expectedResponseCode);
        (await response.HeaderValueAsync(HeaderNames.CacheControl)).ShouldBe("no-cache, no-store");
        (await response.HeaderValueAsync(HeaderNames.Pragma)).ShouldBe("no-cache");
        (await response.HeaderValueAsync(HeaderNames.Age)).ShouldBeNull();

        (await page.Locator("head > meta[name='robots']").GetAttributeAsync("content")).ShouldBe("noindex");

        var errorHeading = page.Locator("body > div > main h2").First;
        (await errorHeading.TextContentAsync()).ShouldBe($"Error: {expectedResponseCode}");

        var errorMessage = errorHeading.Locator("//following-sibling::p[1]");
        (await errorMessage.TextContentAsync()).ShouldBe(expectedMessage);

        (await page.Locator("body > div > main a").GetByText("Back to Home").GetAttributeAsync("href")).ShouldBe("/");
    }
}
