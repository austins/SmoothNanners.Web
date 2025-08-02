using Microsoft.Playwright;
using Microsoft.Playwright.TestAdapter;
using SafeRouting;

namespace SmoothNanners.Web.Tests.Integration;

public abstract class TestBase
{
    private readonly IList<IBrowserContext> _browserContexts = [];

    [ClassDataSource<TestFixture>(Shared = SharedType.PerTestSession)]
    public required TestFixture Fixture { get; init; }

    [After(Test)]
    public async Task AfterTestAsync()
    {
        foreach (var browserContext in _browserContexts)
        {
            await browserContext.DisposeAsync();
        }
    }

    protected async Task<IPage> CreatePageAsync(bool jsEnabled = true)
    {
        var context = await Fixture.Browser.NewContextAsync(
            new BrowserNewContextOptions
            {
                BaseURL = Fixture.BaseUrl,
                JavaScriptEnabled = jsEnabled
            });

        context.SetDefaultTimeout(PlaywrightSettingsProvider.ExpectTimeout ?? 5000);

        _browserContexts.Add(context);

        return await context.NewPageAsync();
    }

    protected string GetPath(IRouteValues route)
    {
        return route.Path(Fixture.LinkGenerator);
    }
}
