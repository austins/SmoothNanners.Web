using AspNetStatic;
using AspNetStatic.Optimizer;
using System.Reflection;

var isSsg = args.HasExitWhenDoneArg();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(o =>
{
    o.ValidateOnBuild = true;
    o.ValidateScopes = true;
});

builder.Logging.AddSimpleConsole(o => o.TimestampFormat = "HH:mm:ss.fff ");

builder.Services.AddRazorPages();

builder.Services.Configure<RouteOptions>(o =>
{
    o.LowercaseUrls = true;
    o.LowercaseQueryStrings = true;
});

// Allow the app's isolated CSS bundle to be produced as a static asset in environments other than Development as SSG is done on the built app instead of a published build.
// We use asp-append-version in place of static asset fingerprinting in order to have fixed paths to fetch for SSG.
builder.WebHost.UseStaticWebAssets();

if (isSsg)
{
    IReadOnlyList<NonPageResource> assetResources =
    [
        new BinResource("/favicon.ico"),
        new CssResource("/assets/vendors/bootswatch/dist/vapor/bootstrap.min.css")
            { OptimizationType = OptimizationType.None },
        new CssResource("/assets/global.css") { OptimizationType = OptimizationType.Css },
        new CssResource($"/{Assembly.GetExecutingAssembly().GetName().Name}.styles.css")
            { OptimizationType = OptimizationType.Css },
        new JsResource("/assets/vendors/alpinejs/dist/cdn.min.js") { OptimizationType = OptimizationType.None },
        new BinResource("/assets/images/avatar.webp")
    ];

    IReadOnlyList<PageResource> pageResources =
    [
        new("/"),
        new("/error")
        {
            Query = $"?code={StatusCodes.Status404NotFound}",
            OutFile = "404.html"
        }
    ];

    builder.Services.AddSingleton<IStaticResourcesInfoProvider>(
        new StaticResourcesInfoProvider([..assetResources, ..pageResources]));
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseStatusCodePagesWithReExecute("/error", "?code={0}").UseStaticFiles().UseRouting();

app.MapRazorPages();

if (isSsg)
{
    var ssgOutputPath = Path.Combine(AppContext.BaseDirectory, "ssg");
    if (Directory.Exists(ssgOutputPath))
    {
        Directory.Delete(ssgOutputPath, true);
    }

    Directory.CreateDirectory(ssgOutputPath);
    app.GenerateStaticContent(ssgOutputPath, true, dontUpdateLinks: true);
}

await app.RunAsync();
