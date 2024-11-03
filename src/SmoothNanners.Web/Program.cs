var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSimpleConsole(o => o.TimestampFormat = "HH:mm:ss.fff ");

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

app.UseStatusCodePagesWithReExecute("/error", "?code={0}").UseStaticFiles().UseRouting().UseOutputCache();

app.MapRazorPages();

await app.RunAsync();

#pragma warning disable S1118
public sealed partial class Program;
#pragma warning restore S1118
