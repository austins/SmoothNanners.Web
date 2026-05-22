using Microsoft.AspNetCore.Http;
using Microsoft.Playwright;
using SmoothNanners.Web.Constants;
using System.Text.RegularExpressions;

namespace SmoothNanners.Web.Tests.E2E.Pages;

public sealed class IndexTests(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task Index_Loads_Success()
    {
        var page = await CreatePageAsync();
        var response = await page.GotoAsync(GetPath(Routes.Pages.Index.Get()));
        response!.Status.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task Index_No_Console_Errors()
    {
        var indexPage = GetPath(Routes.Pages.Index.Get());
        var page = await CreatePageAsync();

        var consoleErrors = new List<string>();
        page.Console += (_, e) =>
        {
            if (e.Type == "error")
            {
                consoleErrors.Add(e.Text);
            }
        };

        await page.GotoAsync(indexPage);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        consoleErrors.Should().BeEmpty();
    }

    [Fact]
    public async Task Index_Title_Is_Correct()
    {
        var page = await CreatePageAsync();
        await page.GotoAsync(GetPath(Routes.Pages.Index.Get()));

        var title = await page.TitleAsync();
        title.Should().Be(AppConstants.SiteName);
    }

    [Fact]
    public async Task Index_Meta_Description_Is_Correct()
    {
        var page = await CreatePageAsync();
        await page.GotoAsync(GetPath(Routes.Pages.Index.Get()));

        var description = await page.Locator("meta[name=\"description\"]").GetAttributeAsync("content");
        description.Should().Be(AppConstants.SiteDescription);
    }

    [Fact]
    public async Task Index_Bio_Section_Renders()
    {
        var page = await CreatePageAsync();
        await page.GotoAsync(GetPath(Routes.Pages.Index.Get()));

        var bio = page.Locator("#bio");
        (await bio.CountAsync()).Should().Be(1);
        await bio.Locator("img").WaitForAsync();
    }

    [Fact]
    public async Task Index_Avatar_Image_Loading_Attributes()
    {
        var page = await CreatePageAsync();
        await page.GotoAsync(GetPath(Routes.Pages.Index.Get()));

        var img = page.Locator("#bio img");

        var src = await img.GetAttributeAsync("src");
        src.Should().Contain("avatar.webp");
        src.Should().Contain("v=");

        var loading = await img.GetAttributeAsync("loading");
        loading.Should().Be("eager");

        var decoding = await img.GetAttributeAsync("decoding");
        decoding.Should().Be("async");

        var alt = await img.GetAttributeAsync("alt");
        alt.Should().Be(AppConstants.SiteName);
    }

    [Fact]
    public async Task Index_Portal_Cards_Render()
    {
        var page = await CreatePageAsync();
        await page.GotoAsync(GetPath(Routes.Pages.Index.Get()));
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var cards = page.Locator(".portal-card");
        (await cards.CountAsync()).Should().Be(4);
    }

    [Fact]
    public async Task Index_YouTube_Embeds_With_JS_Shows_Featured_Heading()
    {
        var page = await CreatePageAsync();
        await page.GotoAsync(GetPath(Routes.Pages.Index.Get()));
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var featuredHeading = page.Locator("h3:has-text(\"Featured\")");
        (await featuredHeading.CountAsync()).Should().Be(1);

        var headingText = await featuredHeading.InnerTextAsync();
        headingText.Should().Contain("Featured");
    }

    [Fact]
    public async Task Index_YouTube_Embeds_With_JS_Thumbnails_Display()
    {
        var page = await CreatePageAsync();
        await page.GotoAsync(GetPath(Routes.Pages.Index.Get()));
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var thumbnails = page.Locator(".youtube-thumbnail");
        (await thumbnails.CountAsync()).Should().Be(2);
    }

    [Fact]
    public async Task Index_YouTube_Embeds_No_JS_Thumbnails_Visible_No_Iframes()
    {
        var page = await CreatePageAsync(false);
        await page.GotoAsync(GetPath(Routes.Pages.Index.Get()));
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var thumbnails = page.Locator(".youtube-thumbnail");
        (await thumbnails.CountAsync()).Should().Be(2);

        var iframes = page.Locator(".youtube-embeds iframe");
        (await iframes.CountAsync()).Should().Be(0);

        var playButtons = page.Locator(".youtube-thumbnail a[aria-label=\"Play YouTube video\"]");
        (await playButtons.CountAsync()).Should().Be(2);
    }

    [Fact]
    public async Task Index_YouTube_Embeds_With_JS_Click_First_Video_Shows_Iframe()
    {
        var page = await CreatePageAsync();
        await page.GotoAsync(GetPath(Routes.Pages.Index.Get()));
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var thumbnails = page.Locator(".youtube-thumbnail");
        var firstPlayButton = thumbnails.Nth(0).Locator("a[aria-label=\"Play YouTube video\"]");
        var firstVideoHref = (await firstPlayButton.GetAttributeAsync("href"))!;
        var firstVideoId = Regex.Match(firstVideoHref, @"v=([a-zA-Z0-9_-]+)").Groups[1].Value!;

        await firstPlayButton.ClickAsync();
        await page.Locator(".youtube-embeds iframe").WaitForAsync();

        (await page.Locator(".youtube-embeds iframe").CountAsync()).Should().Be(1);

        var src = await page.Locator(".youtube-embeds iframe").Nth(0).GetAttributeAsync("src");
        src.Should().Contain(firstVideoId);
    }

    [Fact]
    public async Task Index_YouTube_Embeds_With_JS_Click_Switches_Iframes()
    {
        var page = await CreatePageAsync();
        await page.GotoAsync(GetPath(Routes.Pages.Index.Get()));
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var thumbnails = page.Locator(".youtube-thumbnail");
        var firstPlayButton = thumbnails.Nth(0).Locator("a[aria-label=\"Play YouTube video\"]");
        var secondPlayButton = thumbnails.Nth(1).Locator("a[aria-label=\"Play YouTube video\"]");
        var firstVideoHref = (await firstPlayButton.GetAttributeAsync("href"))!;
        var secondVideoHref = (await secondPlayButton.GetAttributeAsync("href"))!;
        var firstVideoId = Regex.Match(firstVideoHref, @"v=([a-zA-Z0-9_-]+)").Groups[1].Value!;
        var secondVideoId = Regex.Match(secondVideoHref, @"v=([a-zA-Z0-9_-]+)").Groups[1].Value!;

        // Act - click first video
        await firstPlayButton.ClickAsync();
        await page.Locator(".youtube-embeds iframe").WaitForAsync();

        // Assert - first iframe is shown
        var firstSrc = await page.Locator(".youtube-embeds iframe").Nth(0).GetAttributeAsync("src");
        firstSrc.Should().Contain(firstVideoId);

        // Act - click second video
        await secondPlayButton.ClickAsync();
        await page.Locator(".youtube-embeds iframe").WaitForAsync();

        // Assert - second iframe is now shown
        var currentSrc = await page.Locator(".youtube-embeds iframe").Nth(0).GetAttributeAsync("src");
        currentSrc.Should().Contain(secondVideoId);
    }

    [Fact]
    public async Task Index_YouTube_Embeds_With_JS_Only_One_Iframe_Shown_At_Once()
    {
        var page = await CreatePageAsync();
        await page.GotoAsync(GetPath(Routes.Pages.Index.Get()));
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var thumbnails = page.Locator(".youtube-thumbnail");
        var firstPlayButton = thumbnails.Nth(0).Locator("a[aria-label=\"Play YouTube video\"]");
        var secondPlayButton = thumbnails.Nth(1).Locator("a[aria-label=\"Play YouTube video\"]");

        // Act - click first video
        await firstPlayButton.ClickAsync();
        await page.Locator(".youtube-embeds iframe").WaitForAsync();
        (await page.Locator(".youtube-embeds iframe").CountAsync()).Should().Be(1);

        // Act - click second video
        await secondPlayButton.ClickAsync();
        await page.Locator(".youtube-embeds iframe").WaitForAsync();
        (await page.Locator(".youtube-embeds iframe").CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task Index_Youtube_Embeds_Click_Second_Video_Hides_First_Iframe()
    {
        var page = await CreatePageAsync();
        await page.GotoAsync(GetPath(Routes.Pages.Index.Get()));
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var thumbnails = page.Locator(".youtube-thumbnail");
        var firstPlayButton = thumbnails.Nth(0).Locator("a[aria-label=\"Play YouTube video\"]");
        var secondPlayButton = thumbnails.Nth(1).Locator("a[aria-label=\"Play YouTube video\"]");
        var firstVideoHref = (await firstPlayButton.GetAttributeAsync("href"))!;
        var secondVideoHref = (await secondPlayButton.GetAttributeAsync("href"))!;
        var firstVideoId = Regex.Match(firstVideoHref, @"v=([a-zA-Z0-9_-]+)").Groups[1].Value!;
        var secondVideoId = Regex.Match(secondVideoHref, @"v=([a-zA-Z0-9_-]+)").Groups[1].Value!;

        // Act - click first video
        await firstPlayButton.ClickAsync();
        await page.Locator(".youtube-embeds iframe").WaitForAsync();

        // Assert - first iframe is shown
        (await page.Locator(".youtube-embeds iframe").CountAsync()).Should().Be(1);
        var firstSrc = await page.Locator(".youtube-embeds iframe").Nth(0).GetAttributeAsync("src");
        firstSrc.Should().Contain(firstVideoId);

        // Act - click second video
        await secondPlayButton.ClickAsync();
        await page.Locator(".youtube-embeds iframe").WaitForAsync();

        // Assert - first iframe is hidden, second iframe is shown
        (await page.Locator(".youtube-embeds iframe").CountAsync()).Should().Be(1);
        var secondSrc = await page.Locator(".youtube-embeds iframe").Nth(0).GetAttributeAsync("src");
        secondSrc.Should().Contain(secondVideoId);
    }
}
