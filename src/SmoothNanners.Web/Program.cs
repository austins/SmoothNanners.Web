using AspNetStatic;
using AspNetStatic.Optimizer;
using SmoothNanners.Web.Constants;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseDefaultServiceProvider(o =>
{
    o.ValidateOnBuild = true;
    o.ValidateScopes = true;
});

builder.Logging.AddSimpleConsole(o => o.TimestampFormat = "HH:mm:ss.fff ");

// Allow scoped styles to be generated in all environments.
builder.WebHost.UseStaticWebAssets();

builder.Services.AddRazorPages();

var isSsg = args.HasSsgArg();
if (isSsg)
{
    builder.Services.AddSingleton<IStaticResourcesInfoProvider>(
        new StaticResourcesInfoProvider(
        [
            new BinResource("/favicon.ico") { OptimizationType = OptimizationType.None },
            new CssResource("/reset.css"),
            new CssResource($"/{AppConstants.AssemblyName}.styles.css"),
            new JsResource("/vendor/alpinejs/dist/cdn.min.js") { OptimizationType = OptimizationType.None },
            new PageResource("/"),
            new BinResource("/images/avatar.webp") { OptimizationType = OptimizationType.None },
            new PageResource("/notfound") { OutFile = "404.html" }
        ]));
}

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Using static files instead of static assets so exact paths are generated and can be used with SSG.
app.UseStaticFiles().UseRouting();

app.MapRazorPages();

if (isSsg)
{
    var outputPath = Path.Combine(AppContext.BaseDirectory, "ssg");
    if (Directory.Exists(outputPath))
    {
        Directory.Delete(outputPath, true);
    }

    Directory.CreateDirectory(outputPath);
    app.GenerateStaticContent(outputPath, true, dontUpdateLinks: true);
}

await app.RunAsync();
