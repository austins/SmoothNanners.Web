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
        _log.Starting();

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

        _process.OutputDataReceived += (_, e) => LogOutput(e.Data);
        _process.ErrorDataReceived += (_, e) => LogOutput(e.Data);

        _process.Start();
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _log.Stopping();

        if (_process?.HasExited == false)
        {
            await _process.WaitForExitAsync(cancellationToken);
            Dispose();
        }
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

    private void LogOutput(string? output)
    {
        if (output?.Contains("Tailwind CSS: ", StringComparison.Ordinal) == true)
        {
            _log.Output(output.Trim());
        }
    }

    private sealed partial class Log(ILogger logger)
    {
        [LoggerMessage(Level = LogLevel.Information, Message = "Starting Tailwind Watcher...")]
        public partial void Starting();

        [LoggerMessage(Level = LogLevel.Information, Message = "Stopping Tailwind Watcher...")]
        public partial void Stopping();

        [LoggerMessage(Level = LogLevel.Information, Message = "{output}")]
        public partial void Output(string output);
    }
}
#endif
