﻿#if DEBUG
using System.Diagnostics;
using System.Reflection.Metadata;
using SmoothNanners.Web;

[assembly: MetadataUpdateHandler(typeof(HotReloadHandler))]

namespace SmoothNanners.Web;

/// <summary>
/// Handler for hot reload to run Bun build whenever there is a file change when using dotnet watch.
/// See https://learn.microsoft.com/en-us/visualstudio/debugger/hot-reload-metadataupdatehandler?view=vs-2022.
/// </summary>
internal static class HotReloadHandler
{
    /// <summary>
    /// Invoked whenever there is a file change detected pending hot reload.
    /// </summary>
    /// <param name="_">Updated types.</param>
#pragma warning disable SA1313
    public static void UpdateApplication(Type[]? _)
#pragma warning restore SA1313
    {
        if (Environment.GetEnvironmentVariable("DOTNET_WATCH") != "1")
        {
            return;
        }

        RunBunBuild();
    }

    /// <summary>
    /// Run Bun build whenever there is a file change.
    /// </summary>
    private static void RunBunBuild()
    {
        var projectRootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        if (!File.Exists(Path.Combine(projectRootPath, "package.json")))
        {
            Console.WriteLine(
                $"Bun: failed to invoke {nameof(HotReloadHandler)}.{nameof(RunBunBuild)} because project path could not be determined.");

            return;
        }

        var timeout = TimeSpan.FromMilliseconds(3200);

        using var process = new Process();
        process.StartInfo = new ProcessStartInfo("bun", "run build")
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

        return;

        static void LogOutput(string? output)
        {
            if (!string.IsNullOrWhiteSpace(output))
            {
                Console.WriteLine($"Bun: {output}");
            }
        }
    }
}
#endif