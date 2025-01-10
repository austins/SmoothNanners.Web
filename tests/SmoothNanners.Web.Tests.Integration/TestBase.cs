using Microsoft.Playwright;
using Microsoft.Playwright.TestAdapter;
using SafeRouting;

namespace SmoothNanners.Web.Tests.Integration;

[Collection<TestCollection>]
public abstract class TestBase : IAsyncLifetime
{
    private readonly IList<IBrowserContext> _browserContexts = [];
    private readonly TestFixture _fixture;

    protected TestBase(TestFixture fixture)
    {
        _fixture = fixture;
    }

    /// <summary>
    /// Initialization before each test.
    /// </summary>
    /// <returns>ValueTask.</returns>
    public ValueTask InitializeAsync()
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Teardown after each test.
    /// </summary>
    /// <returns>ValueTask.</returns>
    public async ValueTask DisposeAsync()
    {
        foreach (var browserContext in _browserContexts)
        {
            await browserContext.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }

    protected string GetPath(IRouteValues route)
    {
        return route.Path(_fixture.LinkGenerator);
    }

    protected async Task<IPage> CreatePageAsync(bool jsEnabled = true)
    {
        var context = await _fixture.Browser!.NewContextAsync(
            new BrowserNewContextOptions
            {
                BaseURL = _fixture.BaseUrl,
                JavaScriptEnabled = jsEnabled
            });

        context.SetDefaultTimeout(PlaywrightSettingsProvider.ExpectTimeout ?? 5000);

        _browserContexts.Add(context);

        return await context.NewPageAsync();
    }
}
