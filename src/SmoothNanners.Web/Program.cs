using AspNetStatic;
using AspNetStatic.Optimizer;

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

if (isSsg)
{
    builder.Services.AddSingleton<IStaticResourcesInfoProvider>(
        new StaticResourcesInfoProvider(
        [
            new BinResource("/favicon.ico"),
            new CssResource("/app.css") { OptimizationType = OptimizationType.None },
            new JsResource("/vendors/alpinejs/dist/cdn.min.js") { OptimizationType = OptimizationType.None },
            new PageResource("/"),
            new BinResource("/images/avatar.webp"),
            new PageResource("/error")
            {
                Query = $"?code={StatusCodes.Status404NotFound}",
                OutFile = "404.html"
            }
        ]));
}
#if DEBUG
else if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHostedService<SmoothNanners.Web.TailwindWatcher>();
}
#endif

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
    app.GenerateStaticContent(ssgOutputPath, true);
}

await app.RunAsync();

#pragma warning disable S1118
public sealed partial class Program;
#pragma warning restore S1118
