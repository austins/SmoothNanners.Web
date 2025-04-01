#if DEBUG
using System.Diagnostics;

namespace SmoothNanners.Web;

internal sealed partial class TailwindWatcher
    : IHostedService,
        IDisposable
{
    private readonly Log _log;
    private readonly Process _process;

    public TailwindWatcher(IHostEnvironment environment, ILogger<TailwindWatcher> logger)
    {
        _log = new Log(logger);

        _process = new Process
        {
            StartInfo = new ProcessStartInfo(
                "dotnet",
                "tool run tailwindcss watch -t v4.0.17 -m -i tailwind.css -o wwwroot/app.css")
            {
                WorkingDirectory = environment.ContentRootPath,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };

        _process.OutputDataReceived += LogOutput;
        _process.ErrorDataReceived += LogOutput;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _process.Start();
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();

        _log.Started();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (!_process.HasExited)
        {
            _process.Kill();
        }

        _process.Dispose();
    }

    private void LogOutput(object _, DataReceivedEventArgs e)
    {
        if (e.Data?.Contains("Tailwind CSS: ", StringComparison.Ordinal) == true)
        {
            _log.Output(e.Data.Trim());
        }
    }

    private sealed partial class Log(ILogger logger)
    {
        [LoggerMessage(Level = LogLevel.Information, Message = "Started Tailwind Watcher.")]
        public partial void Started();

        [LoggerMessage(Level = LogLevel.Information, Message = "{output}")]
        public partial void Output(string output);
    }
}
#endif
