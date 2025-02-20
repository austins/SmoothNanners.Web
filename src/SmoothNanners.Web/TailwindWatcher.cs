#if DEBUG
using System.Diagnostics;

namespace SmoothNanners.Web;

internal sealed partial class TailwindWatcher
    : IHostedService,
        IDisposable
{
    private readonly IHostEnvironment _environment;
    private readonly Log _log;
    private Process? _process;

    public TailwindWatcher(IHostEnvironment environment, ILogger<TailwindWatcher> logger)
    {
        _environment = environment;
        _log = new Log(logger);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _process = new Process
        {
            StartInfo = new ProcessStartInfo(
                "dotnet",
                "tool run tailwindcss watch -i tailwind.css -o wwwroot/app.css --minify")
            {
                WorkingDirectory = _environment.ContentRootPath,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };

        _process.OutputDataReceived += LogOutput;
        _process.ErrorDataReceived += LogOutput;

        _process.Start();
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();

        _log.Started();

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_process?.HasExited == false)
        {
            await _process.WaitForExitAsync(cancellationToken);
            Dispose();
        }

        _log.Stopped();
    }

    public void Dispose()
    {
        if (_process is not null)
        {
            _process.Kill();
            _process.Dispose();
            _process = null;
        }
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

        [LoggerMessage(Level = LogLevel.Information, Message = "Stopped Tailwind Watcher.")]
        public partial void Stopped();

        [LoggerMessage(Level = LogLevel.Information, Message = "{output}")]
        public partial void Output(string output);
    }
}
#endif
