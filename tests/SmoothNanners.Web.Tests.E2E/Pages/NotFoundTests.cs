using Microsoft.AspNetCore.Http;
using Microsoft.Playwright;
using SmoothNanners.Web.Constants;

namespace SmoothNanners.Web.Tests.E2E.Pages;

public sealed class NotFoundTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task NotFound_Loads_Success()
    {
        var page = await CreatePageAsync();
        var response = await page.GotoAsync(GetPath(Routes.Pages.NotFound.Get()));
        response!.Status.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task NotFound_Title_Is_Error_404()
    {
        var page = await CreatePageAsync();
        await page.GotoAsync(GetPath(Routes.Pages.NotFound.Get()));
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var title = await page.TitleAsync();
        title.Should().Be($"Error: 404 | {AppConstants.SiteName}");
    }

    [Fact]
    public async Task NotFound_Error_Message_Is_Displayed()
    {
        var page = await CreatePageAsync();
        await page.GotoAsync(GetPath(Routes.Pages.NotFound.Get()));
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var heading = page.Locator("h2");
        (await heading.CountAsync()).Should().Be(1);

        var headingText = await heading.InnerTextAsync();
        headingText.Should().Be("Error: 404");
    }

    [Fact]
    public async Task NotFound_Message_Is_Displayed()
    {
        var page = await CreatePageAsync();
        await page.GotoAsync(GetPath(Routes.Pages.NotFound.Get()));
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var message = page.Locator("p");
        (await message.CountAsync()).Should().Be(1);

        var messageText = await message.InnerTextAsync();
        messageText.Should().Be("The page you are looking for was not found.");
    }

    [Fact]
    public async Task NotFound_Back_To_Home_Link_Is_Correct()
    {
        var page = await CreatePageAsync();
        await page.GotoAsync(GetPath(Routes.Pages.NotFound.Get()));
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var backLink = page.Locator("a:has-text(\"Back to Home\")");
        (await backLink.CountAsync()).Should().Be(1);

        var href = await backLink.Nth(0).GetAttributeAsync("href");
        href.Should().Be("/");

        var linkText = await backLink.Nth(0).InnerTextAsync();
        linkText.Should().Be("Back to Home");
    }

    [Fact]
    public async Task NotFound_Noindex_Meta_Tag_Is_Present()
    {
        var page = await CreatePageAsync();
        await page.GotoAsync(GetPath(Routes.Pages.NotFound.Get()));
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var robots = page.Locator("meta[name=\"robots\"]");
        (await robots.CountAsync()).Should().Be(1);

        var content = await robots.Nth(0).GetAttributeAsync("content");
        content.Should().Be("noindex");
    }

    [Fact]
    public async Task NotFound_Back_To_Home_Link_Navigates_To_Index()
    {
        var page = await CreatePageAsync();
        await page.GotoAsync(GetPath(Routes.Pages.NotFound.Get()));
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var backLink = page.Locator("a:has-text(\"Back to Home\")");
        await backLink.ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var uri = new Uri(page.Url);
        uri.PathAndQuery.Should().Be(GetPath(Routes.Pages.Index.Get()));

        var bio = page.Locator("#bio");
        await bio.WaitForAsync();
    }

    [Fact]
    public async Task NotFound_No_Console_Errors()
    {
        var notFoundPath = GetPath(Routes.Pages.NotFound.Get());
        var page = await CreatePageAsync();

        var consoleErrors = new List<string>();
        page.Console += (_, e) =>
        {
            if (e.Type != "error")
            {
                return;
            }

            var text = e.Text;
            if (text.Contains("Failed to load resource", StringComparison.OrdinalIgnoreCase)
                && text.Contains("404", StringComparison.OrdinalIgnoreCase)
                && page.Url.Contains(notFoundPath, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            consoleErrors.Add(text);
        };

        await page.GotoAsync(GetPath(Routes.Pages.NotFound.Get()));
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        consoleErrors.Should().BeEmpty();
    }

    [Fact]
    public async Task NotFound_Header_Hierarchy_Is_Correct()
    {
        var page = await CreatePageAsync();
        await page.GotoAsync(GetPath(Routes.Pages.NotFound.Get()));
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var h1 = page.Locator("h1");
        (await h1.CountAsync()).Should().Be(1);

        var h1Text = await h1.Nth(0).InnerTextAsync();
        h1Text.Should().Be(AppConstants.SiteName);

        var h2 = page.Locator("h2");
        (await h2.CountAsync()).Should().Be(1);

        var h2Text = await h2.Nth(0).InnerTextAsync();
        h2Text.Should().Be("Error: 404");
    }
}
