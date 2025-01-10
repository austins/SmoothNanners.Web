using Microsoft.AspNetCore.Routing;
using Microsoft.Playwright;
using Microsoft.Playwright.TestAdapter;

namespace SmoothNanners.Web.Tests.Integration;

/// <summary>
/// Test fixture used for <see cref="TestCollection" /> that ensures it's instantiated
/// only once as a singleton for any tests within the collection.
/// </summary>
public sealed class TestFixture : IAsyncLifetime
{
    private readonly AppFactory _appFactory = new();
    private IPlaywright? _playwright;

    public string BaseUrl => _appFactory.BaseUrl;

    public LinkGenerator LinkGenerator => _appFactory.LinkGenerator;

    public IBrowser? Browser { get; private set; }

    /// <summary>
    /// Initialization that should run once before all tests at the creation time.
    /// </summary>
    /// <returns>ValueTask.</returns>
    public async ValueTask InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();

        Browser = await _playwright[PlaywrightSettingsProvider.BrowserName].LaunchAsync();
    }

    /// <summary>
    /// Async teardown that should run once after all tests are finished.
    /// </summary>
    /// <returns>ValueTask.</returns>
    public async ValueTask DisposeAsync()
    {
        if (Browser is not null)
        {
            await Browser.DisposeAsync();
        }

        _playwright?.Dispose();
        await _appFactory.DisposeAsync();
    }
}

[CollectionDefinition]
public sealed class TestCollection : ICollectionFixture<TestFixture>;
