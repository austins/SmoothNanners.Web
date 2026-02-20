using Microsoft.Playwright;
using Microsoft.Playwright.TestAdapter;
using SafeRouting;

namespace SmoothNanners.Web.Tests.Integration;

public abstract class TestBase : IAsyncDisposable
{
    private readonly TestFixture _fixture;
    private readonly IList<IBrowserContext> _browserContexts = [];

    protected TestBase(TestFixture fixture)
    {
        _fixture = fixture;
    }

    /// <summary>
    /// Disposal that runs after each test method is run.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        foreach (var browserContext in _browserContexts)
        {
            await browserContext.DisposeAsync();
        }
    }

    protected async Task<IPage> CreatePageAsync(bool jsEnabled = true)
    {
        var context = await _fixture.Browser.NewContextAsync(
            new BrowserNewContextOptions
            {
                BaseURL = _fixture.AppFactory.BaseUrl,
                JavaScriptEnabled = jsEnabled
            });

        context.SetDefaultTimeout(PlaywrightSettingsProvider.ExpectTimeout ?? 5000);

        _browserContexts.Add(context);

        return await context.NewPageAsync();
    }

    protected string GetPath(IRouteValues route)
    {
        return route.Path(_fixture.AppFactory.LinkGenerator);
    }
}
