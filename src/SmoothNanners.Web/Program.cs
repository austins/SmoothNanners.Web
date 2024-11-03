using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSimpleConsole(o => o.TimestampFormat = "HH:mm:ss.fff ");

builder.Services.AddWebOptimizer(
    p =>
    {
        p.MinifyCssFiles("~/assets/styles/**/*.css");
        p.MinifyJsFiles("~/assets/scripts/**/*.js");
    },
    o =>
    {
        o.EnableDiskCache = false;
        o.HttpsCompression = HttpsCompressionMode.DoNotCompress;
    });

builder.Services.AddRazorPages();

builder.Services.Configure<RouteOptions>(
    o =>
    {
        o.LowercaseUrls = true;
        o.LowercaseQueryStrings = true;
    });

builder.Services.AddOutputCache();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app
    .UseStatusCodePagesWithReExecute("/error", "?code={0}")
    .UseWebOptimizer()
    .UseStaticFiles()
    .UseRouting()
    .UseOutputCache();

app.MapRazorPages();

await app.RunAsync();

#pragma warning disable S1118
public sealed partial class Program;
#pragma warning restore S1118
