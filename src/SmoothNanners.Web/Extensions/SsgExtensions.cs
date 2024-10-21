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
                new PageResource("/error")
                {
                    Query = "?code=404",
                    OutFile = "404.html"
                },
                new CssResource("/assets/vendors/bootswatch/dist/vapor/bootstrap.min.css")
                {
                    OptimizerType = OptimizerType.None
                },
                new CssResource("/assets/styles/main.css"),
                new JsResource("/assets/scripts/youtube-embed.js"),
                new BinResource("/favicon.ico") { OptimizerType = OptimizerType.None },
                new BinResource("/assets/images/avatar.jpg") { OptimizerType = OptimizerType.None }
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
