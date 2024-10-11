using AspNetStatic;
using SmoothNanners.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSimpleConsole(o => o.TimestampFormat = "HH:mm:ss.fff ");

builder.Services.AddRazorPages();

builder.Services.Configure<RouteOptions>(
    o =>
    {
        o.LowercaseUrls = true;
        o.LowercaseQueryStrings = true;
    });

if (args.HasExitWhenDoneArg())
{
    builder.Services.AddSsg();
}

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

// Not using MapStaticAssets because we are doing SSG.
app.UseStatusCodePagesWithReExecute("/error", "?code={0}").UseStaticFiles().UseRouting();

app.MapRazorPages();

if (args.HasExitWhenDoneArg())
{
    app.RunSsgAndExit();
}

await app.RunAsync();

#pragma warning disable S1118
public sealed partial class Program;
#pragma warning restore S1118
