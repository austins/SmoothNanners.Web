using Meziantou.Xunit.v3;
using Microsoft.AspNetCore.Routing;
using Microsoft.Playwright;
using Microsoft.Playwright.TestAdapter;
using Microsoft.Testing.Platform.Services;

namespace SmoothNanners.Web.Tests.Integration;

/// <summary>
/// Test fixture that should be configured as an assembly fixture that sets up
/// services used across all tests before and after all tests are run.
/// </summary>
public sealed class TestFixture : IAsyncLifetime
{
    private readonly AppFactory _appFactory = new();
    private IPlaywright? _playwright;

    public Uri BaseUrl => _appFactory.ClientOptions.BaseAddress;

    public LinkGenerator LinkGenerator => field ??= _appFactory.Services.GetRequiredService<LinkGenerator>();

    public IBrowser Browser { get; private set; } = null!;

    public async ValueTask InitializeAsync()
    {
        // Install browser binaries needed for Playwright.
        var exitCode = Microsoft.Playwright.Program.Main(["install", PlaywrightSettingsProvider.BrowserName]);
        if (exitCode != 0)
        {
            throw new InvalidOperationException($"Playwright browser install exited with code [{exitCode}].");
        }

        _appFactory.StartServer();

#pragma warning disable IDISP003
        _playwright = await Playwright.CreateAsync();
#pragma warning restore IDISP003

        Browser = await _playwright[PlaywrightSettingsProvider.BrowserName].LaunchAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await Browser.DisposeAsync();
        _playwright?.Dispose();
        await _appFactory.DisposeAsync();
    }
}

[CollectionDefinition(nameof(TestCollection))]
[EnableParallelization]
#pragma warning disable CA1711
public sealed class TestCollection : ICollectionFixture<TestFixture>;
#pragma warning restore CA1711
