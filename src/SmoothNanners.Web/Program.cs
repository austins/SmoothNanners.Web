using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SmoothNanners.Web.Components;
using SmoothNanners.Web.Extensions;
using SmoothNanners.Web.Telemetry;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSimpleConsole(o => o.TimestampFormat = "HH:mm:ss.fff ");

builder.AddTelemetry();

builder.Services.AddRazorComponents();

builder.Services.AddOutputCache();

builder.Services.AddRateLimiting().AddHealthChecks();

#if DEBUG
if (Environment.GetEnvironmentVariable("DOTNET_WATCH") == "1")
{
    builder.Services.AddHostedService<SmoothNanners.Web.TailwindWatcher>();
}
#endif

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app
    .UseStatusCodePagesWithReExecute("/error", "?code={0}")
    .UseRouting()
    .UseOutputCache()
    .UseRateLimiter()
    .UseAntiforgery();

app
    .MapHealthChecks(
        "/_health",
        new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse })
    .RequireRateLimiting(RateLimitingExtensions.HealthCheckRateLimiterPolicyName);

app.MapStaticAssets();

app.MapRazorComponents<App>();

await app.RunAsync();

#pragma warning disable S1118
public sealed partial class Program;
#pragma warning restore S1118
