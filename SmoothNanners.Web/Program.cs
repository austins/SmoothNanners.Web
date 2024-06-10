using AspNetStatic;
using SmoothNanners.Web.Extensions;
using SmoothNanners.Web.Vite;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSimpleConsole(o => o.TimestampFormat = "HH:mm:ss.fff ");

builder
    .Services
    .AddOptions<ViteOptions>()
    .Bind(builder.Configuration.GetRequiredSection(ViteOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

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

app.UseStaticFiles().UseStatusCodePagesWithReExecute("/error", "?code={0}").UseRouting();

app.MapRazorPages();

if (args.HasExitWhenDoneArg())
{
    app.RunSsgAndExit();
}

await app.RunAsync();

public sealed partial class Program
{
}
