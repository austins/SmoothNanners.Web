using AspNetStatic;

namespace SmoothNanners.Web.Extensions;

internal static class SsgExtensions
{
    public static IServiceCollection AddSsg(this IServiceCollection services)
    {
        return services.AddSingleton<IStaticResourcesInfoProvider>(
            new StaticResourcesInfoProvider(
            [
                new PageResource("/"),
                new PageResource("/error?code=404") { OutFile = "404.html" },
                new CssResource("/assets/main.css") { OptimizerType = OptimizerType.None },
                new JsResource("/assets/main.js") { OptimizerType = OptimizerType.None },
                new BinResource("/favicon.ico") { OptimizerType = OptimizerType.None },
                new BinResource("/images/avatar.jpg") { OptimizerType = OptimizerType.None }
            ]));
    }

    public static void RunSsgAndExit(this WebApplication app)
    {
        // Ensure output directory exists.
        var ssgOutputPath = Path.Combine(app.Environment.ContentRootPath, "bin/ssg");
        if (Directory.Exists(ssgOutputPath))
        {
            Directory.Delete(ssgOutputPath, true);
        }

        Directory.CreateDirectory(ssgOutputPath);

#pragma warning disable CA1848
        app.Logger.LogInformation("Running SSG and exiting...");
#pragma warning restore CA1848

        app.GenerateStaticContent(Path.Combine(app.Environment.ContentRootPath, "bin/ssg"), true);
    }
}
