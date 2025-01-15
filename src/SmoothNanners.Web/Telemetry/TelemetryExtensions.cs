using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace SmoothNanners.Web.Telemetry;

internal static class TelemetryExtensions
{
    public static void AddTelemetry(this WebApplicationBuilder builder)
    {
        var options = builder.Configuration.GetSection(TelemetryOptions.SectionName).Get<TelemetryOptions>();
        if (options?.Enabled != true)
        {
            return;
        }

        builder.Logging.AddOpenTelemetry(
            o =>
            {
                o.IncludeFormattedMessage = true;
                o.IncludeScopes = true;
                o.AddOtlpExporter();
            });

        builder
            .Services
            .AddOpenTelemetry()
            .ConfigureResource(
                b => b
                    .AddService(builder.Environment.ApplicationName)
                    .AddAttributes(
                        new Dictionary<string, object>
                        {
                            ["deployment.environment"] = builder.Environment.EnvironmentName
                        }))
            .WithMetrics(
                b => b
                    .AddProcessInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter())
            .WithTracing(b => b.AddAspNetCoreInstrumentation().AddOtlpExporter());
    }
}
