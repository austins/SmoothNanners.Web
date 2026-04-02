using System.Diagnostics;

namespace SmoothNanners.Web;

internal sealed partial class ViteWatcher
    : IHostedService,
        IDisposable
{
    private readonly Log _log;
    private readonly Process _process;

    public ViteWatcher(IHostEnvironment environment, ILogger<ViteWatcher> logger)
    {
        _log = new Log(logger);

        _process = new Process
        {
            StartInfo = new ProcessStartInfo("npm", "run build")
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
        if (!string.IsNullOrWhiteSpace(e.Data))
        {
            _log.Output(e.Data.Trim());
        }
    }

    private sealed partial class Log(ILogger logger)
    {
        [LoggerMessage(Level = LogLevel.Information, Message = "Started Vite Watcher.")]
        public partial void Started();

        [LoggerMessage(Level = LogLevel.Information, Message = "{output}")]
        public partial void Output(string output);
    }
}
