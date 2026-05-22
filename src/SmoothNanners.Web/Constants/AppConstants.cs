using System.Reflection;

namespace SmoothNanners.Web.Constants;

internal static class AppConstants
{
    public const string SiteName = "SmoothNanners";

    public const string SiteDescription = "SmoothNanners is a gamer, musician, software developer, and photographer.";

    public static readonly string AssemblyName = Assembly.GetExecutingAssembly().GetName().Name!;
}
