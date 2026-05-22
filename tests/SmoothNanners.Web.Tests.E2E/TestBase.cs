using Meziantou.Xunit.v3;
using Microsoft.Playwright;
using SafeRouting;

namespace SmoothNanners.Web.Tests.E2E;

[Collection(nameof(TestCollection))]
[EnableParallelization]
public abstract class TestBase
{
    private readonly TestFixture _fixture;

    protected TestBase(TestFixture fixture)
    {
        _fixture = fixture;
    }

    protected string GetPath(IRouteValues routeValues)
    {
        return routeValues.Path(_fixture.LinkGenerator!);
    }

    protected Task<IPage> CreatePageAsync(bool isJsEnabled = true)
    {
        return _fixture.Browser!.NewPageAsync(
            new BrowserNewPageOptions
            {
                BaseURL = _fixture.BaseAddress!.AbsoluteUri,
                JavaScriptEnabled = isJsEnabled
            });
    }
}
