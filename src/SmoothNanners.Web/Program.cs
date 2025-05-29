using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SmoothNanners.Web.Extensions;
using SmoothNanners.Web.Telemetry;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(o =>
{
    o.ValidateOnBuild = true;
    o.ValidateScopes = true;
});

builder.Logging.AddSimpleConsole(o => o.TimestampFormat = "HH:mm:ss.fff ");

builder.AddTelemetry();

builder.Services.AddControllersWithViews();

builder.Services.Configure<RouteOptions>(o =>
{
    o.LowercaseUrls = true;
    o.LowercaseQueryStrings = true;
});

builder.Services.AddOutputCache();

builder.Services.AddRateLimiting().AddHealthChecks();

#if DEBUG
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHostedService<SmoothNanners.Web.TailwindWatcher>();
}
#endif

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app.UseStatusCodePagesWithReExecute("/error", "?code={0}").UseRouting();

if (!app.Environment.IsDevelopment())
{
    app.UseOutputCache();
}

app.UseRateLimiter();

app
    .MapHealthChecks(
        "/_health",
        new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse })
    .RequireRateLimiting(RateLimitingExtensions.HealthCheckRateLimiterPolicyName);

app.MapStaticAssets();

app.MapControllers().WithStaticAssets();

await app.RunAsync();

#pragma warning disable S1118
public sealed partial class Program;
#pragma warning restore S1118
