using Microsoft.Playwright;
using Microsoft.Playwright.TestAdapter;

namespace SmoothNanners.Web.Tests.Integration;

public sealed class TestFixture : IAsyncLifetime
{
    private IPlaywright? _playwright;

    public AppFactory AppFactory { get; } = new();

    public IBrowser Browser { get; private set; } = null!;

    public async ValueTask InitializeAsync()
    {
#pragma warning disable IDISP003
        _playwright = await Playwright.CreateAsync();
#pragma warning restore IDISP003

        Browser = await _playwright[PlaywrightSettingsProvider.BrowserName].LaunchAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await Browser.DisposeAsync();
        _playwright?.Dispose();
        await AppFactory.DisposeAsync();
    }
}
