using Microsoft.AspNetCore.Routing;
using Microsoft.Playwright;
using Microsoft.Playwright.TestAdapter;

namespace SmoothNanners.Web.Tests.E2E;

/// <summary>
/// Test fixture used for <see cref="TestCollection" /> that ensures it's instantiated
/// only once as a singleton for any tests within the collection.
/// </summary>
public sealed class TestFixture
    : IAsyncLifetime,
        IDisposable
{
    private readonly AppFactory _appFactory = new();
    private IPlaywright? _playwright;

    public string BaseUrl => _appFactory.BaseUrl;

    public LinkGenerator LinkGenerator => _appFactory.LinkGenerator;

    public IBrowser? Browser { get; private set; }

    /// <summary>
    /// Initialization that should run once before all tests at the creation time.
    /// </summary>
    /// <returns>Task.</returns>
    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        Browser = await _playwright[PlaywrightSettingsProvider.BrowserName]
            .LaunchAsync(PlaywrightSettingsProvider.LaunchOptions);
    }

    /// <summary>
    /// Async teardown that should run once after all tests are finished.
    /// </summary>
    /// <returns>Task.</returns>
    public async Task DisposeAsync()
    {
        if (Browser is not null)
        {
            await Browser.DisposeAsync();
        }
    }

    /// <summary>
    /// Teardown that should run once after all tests are finished.
    /// </summary>
    public void Dispose()
    {
        _playwright?.Dispose();
        _appFactory.Dispose();
    }
}

[CollectionDefinition(nameof(TestCollection))]
public sealed class TestCollection : ICollectionFixture<TestFixture>
{
}
