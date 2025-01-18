using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Features;
using SmoothNanners.Web.Extensions;
using SmoothNanners.Web.Telemetry;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSimpleConsole(o => o.TimestampFormat = "HH:mm:ss.fff ");

builder.AddTelemetry();

if (!builder.Environment.IsDevelopment())
{
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
}

builder.Services.AddRazorPages();

builder.Services.Configure<RouteOptions>(
    o =>
    {
        o.LowercaseUrls = true;
        o.LowercaseQueryStrings = true;
    });

builder.Services.AddOutputCache();

builder.Services.AddRateLimiting().AddHealthChecks();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app.UseStatusCodePagesWithReExecute("/error", "?code={0}");

if (!app.Environment.IsDevelopment())
{
    app.UseWebOptimizer();
}

app.UseStaticFiles().UseRouting().UseOutputCache().UseRateLimiter();

app
    .MapHealthChecks(
        "/_health",
        new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse })
    .RequireRateLimiting(RateLimitingExtensions.HealthCheckRateLimiterPolicyName);

app.MapRazorPages();

await app.RunAsync();

#pragma warning disable S1118
public sealed partial class Program;
#pragma warning restore S1118
