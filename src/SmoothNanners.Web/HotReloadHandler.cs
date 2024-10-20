#if DEBUG
using SmoothNanners.Web;
using System.Diagnostics;
using System.Reflection.Metadata;

[assembly: MetadataUpdateHandler(typeof(HotReloadHandler))]

namespace SmoothNanners.Web;

/// <summary>
/// Handler for hot reload to build Tailwind CSS whenever there is a file change when using dotnet watch.
/// See https://learn.microsoft.com/en-us/visualstudio/debugger/hot-reload-metadataupdatehandler?view=vs-2022.
/// </summary>
internal static class HotReloadHandler
{
    /// <summary>
    /// Invoked whenever there is a file change detected pending hot reload.
    /// </summary>
    /// <param name="_">Updated types. Not used.</param>
    public static void UpdateApplication(Type[]? _)
    {
        if (Environment.GetEnvironmentVariable("DOTNET_WATCH") != "1")
        {
            return;
        }

        BuildTailwindCss();
    }

    /// <summary>
    /// Build Tailwind CSS whenever there is a file change.
    /// </summary>
    private static void BuildTailwindCss()
    {
        // Ensure we're using the correct project root path.
        var projectRootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var tailwindConfigPath = Path.Combine(projectRootPath, "tailwind.config.js");
        if (!File.Exists(tailwindConfigPath))
        {
            Console.WriteLine(
                $"Tailwind: failed to invoke {nameof(HotReloadHandler)}.{nameof(BuildTailwindCss)} because project path could not be determined.");

            return;
        }

        var timeout = TimeSpan.FromMilliseconds(3200);

        using var process = new Process();
        process.StartInfo =
            new ProcessStartInfo("dotnet", "tool run tailwindcss -i tailwind.css -o ./wwwroot/assets/main.min.css -m")
            {
                WorkingDirectory = projectRootPath,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

        process.OutputDataReceived += (_, e) => LogOutput(e.Data);
        process.ErrorDataReceived += (_, e) => LogOutput(e.Data);

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        if (!process.WaitForExit(timeout))
        {
            process.Kill();
        }
    }

    private static void LogOutput(string? output)
    {
        if (!string.IsNullOrWhiteSpace(output))
        {
            Console.WriteLine($"Tailwind: {output}");
        }
    }
}
#endif
