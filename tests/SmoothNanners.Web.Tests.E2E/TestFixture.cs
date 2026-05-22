using Meziantou.Xunit.v3;
using Microsoft.AspNetCore.Routing;
using Microsoft.Playwright;
using Microsoft.Playwright.TestAdapter;
using Microsoft.Testing.Platform.Services;

namespace SmoothNanners.Web.Tests.E2E;

public sealed class TestFixture : IAsyncLifetime
{
    private AppFactory? _appFactory;
    private IPlaywright? _playwright;

    public async ValueTask InitializeAsync()
    {
        var exitCode = Microsoft.Playwright.Program.Main(["install", PlaywrightSettingsProvider.BrowserName]);
        if (exitCode != 0)
        {
            throw new InvalidOperationException($"Failed to install Playwright browser. Exit code: {exitCode}");
        }

#pragma warning disable IDISP003
        _appFactory = new AppFactory();
        _playwright = await Playwright.CreateAsync();
#pragma warning restore IDISP003

        BaseAddress = _appFactory.ClientOptions.BaseAddress;
        LinkGenerator = _appFactory.Services.GetRequiredService<LinkGenerator>();
        Browser = await _playwright[PlaywrightSettingsProvider.BrowserName].LaunchAsync();
    }

    public Uri? BaseAddress { get; private set; }

    public LinkGenerator? LinkGenerator { get; private set; }

    public IBrowser? Browser { get; private set; }

    public async ValueTask DisposeAsync()
    {
        if (Browser is not null)
        {
            await Browser.DisposeAsync();
        }

        _playwright?.Dispose();

        if (_appFactory is not null)
        {
            await _appFactory.DisposeAsync();
        }
    }
}

[CollectionDefinition(nameof(TestCollection))]
[EnableParallelization]
#pragma warning disable CA1711
public sealed class TestCollection : ICollectionFixture<TestFixture>;
#pragma warning restore CA1711
