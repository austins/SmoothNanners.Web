using Microsoft.Playwright.TestAdapter;
using Xunit.Sdk;
using Xunit.v3;

namespace SmoothNanners.Web.Tests.Integration;

/// <summary>
/// Setup for the assembly that runs early in the test pipeline during discovery and execution.
/// </summary>
internal sealed class AssemblySetup : ITestPipelineStartup
{
    public ValueTask StartAsync(IMessageSink diagnosticMessageSink)
    {
        // Install browser binaries needed for Playwright.
        var exitCode = Microsoft.Playwright.Program.Main(["install", PlaywrightSettingsProvider.BrowserName]);
        if (exitCode != 0)
        {
            throw new InvalidOperationException($"Playwright browser install exited with code [{exitCode}].");
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask StopAsync()
    {
        return ValueTask.CompletedTask;
    }
}
