using Microsoft.AspNetCore.Routing;
using Microsoft.Playwright;
using Microsoft.Playwright.TestAdapter;
using TUnit.Core.Interfaces;

namespace SmoothNanners.Web.Tests.Integration;

/// <summary>
/// Test fixture that should be instantiated only once as a singleton for any tests within the session.
/// </summary>
public sealed class TestFixture
    : IAsyncInitializer,
        IAsyncDisposable
{
    private readonly AppFactory _appFactory = new();
    private IPlaywright? _playwright;

    public string BaseUrl => _appFactory.BaseUrl;

    public LinkGenerator LinkGenerator => _appFactory.LinkGenerator;

    public IBrowser Browser { get; private set; } = null!;

    /// <summary>
    /// Initialization that should run once when the test session starts.
    /// </summary>
    /// <returns>Task.</returns>
    public async Task InitializeAsync()
    {
#pragma warning disable IDISP003
        _playwright = await Playwright.CreateAsync();
#pragma warning restore IDISP003

        Browser = await _playwright[PlaywrightSettingsProvider.BrowserName].LaunchAsync();
    }

    /// <summary>
    /// Async teardown that should run once the test session is finished.
    /// </summary>
    /// <returns>ValueTask.</returns>
    public async ValueTask DisposeAsync()
    {
        await Browser.DisposeAsync();
        _playwright?.Dispose();
        await _appFactory.DisposeAsync();
    }
}
