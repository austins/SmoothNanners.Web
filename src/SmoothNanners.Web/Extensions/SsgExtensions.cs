using AspNetStatic;

namespace SmoothNanners.Web.Extensions;

internal static class SsgExtensions
{
    public static IServiceCollection AddSsg(this IServiceCollection services)
    {
        // Specify pages to prerender.
        return services.AddSingleton<IStaticResourcesInfoProvider>(
            new StaticResourcesInfoProvider(
            [
                new PageResource("/"),
                new PageResource("/error")
                {
                    Query = "?code=404",
                    OutFile = "404.html"
                }
            ]));
    }

    public static void RunSsgAndExit(this WebApplication app)
    {
        // Ensure output directory exists.
        var ssgOutputPath = Path.Combine(app.Environment.ContentRootPath, "ssg");
        if (Directory.Exists(ssgOutputPath))
        {
            Directory.Delete(ssgOutputPath, true);
        }

        Directory.CreateDirectory(ssgOutputPath);

#pragma warning disable CA1848
        app.Logger.LogInformation("Running SSG and exiting...");
#pragma warning restore CA1848

        CopyWebRootContents(app.Environment.WebRootPath, ssgOutputPath);

        app.GenerateStaticContent(ssgOutputPath, true);
    }

    private static void CopyWebRootContents(string webRootPath, string ssgOutputPath)
    {
        foreach (var directoryPath in Directory.GetDirectories(webRootPath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(directoryPath.Replace(webRootPath, ssgOutputPath, StringComparison.Ordinal));
        }

        foreach (var filePath in Directory.GetFiles(webRootPath, "*.*", SearchOption.AllDirectories))
        {
            File.Copy(filePath, filePath.Replace(webRootPath, ssgOutputPath, StringComparison.Ordinal));
        }
    }
}
