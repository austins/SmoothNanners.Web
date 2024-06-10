using AspNetStatic;

namespace SmoothNanners.Web.Extensions;

internal static class SsgExtensions
{
    public static IServiceCollection AddSsg(this IServiceCollection services)
    {
        return services.AddSingleton<IStaticResourcesInfoProvider>(
            new StaticResourcesInfoProvider(
                [new PageResource("/"), new PageResource("/error?code=404") { OutFile = "404.html" }]));
    }

    public static void RunSsgAndExit(this WebApplication app)
    {
        // Create output directory.
        var outputSsgPath = Path.Combine(app.Environment.ContentRootPath, "bin/ssg");
        if (Directory.Exists(outputSsgPath))
        {
            Directory.Delete(outputSsgPath, true);
        }

        Directory.CreateDirectory(outputSsgPath);

        // Copy all assets to the output directory.
        var assetPaths = Directory.GetFiles(app.Environment.WebRootPath, "*", SearchOption.AllDirectories);
        foreach (var assetPath in assetPaths)
        {
            var relativeFilePath = Path.GetRelativePath(app.Environment.WebRootPath, assetPath);
            Directory.CreateDirectory(Path.Combine(outputSsgPath, Path.GetDirectoryName(relativeFilePath)!));
            File.Copy(assetPath, Path.Combine(outputSsgPath, Path.Combine(outputSsgPath, relativeFilePath)));
        }

        // Generate static pages.
        app.GenerateStaticContent(outputSsgPath, true);
    }
}
