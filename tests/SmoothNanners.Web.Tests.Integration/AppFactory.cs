using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Testing.Platform.Services;
using System.Net;
using System.Net.Sockets;

namespace SmoothNanners.Web.Tests.Integration;

internal sealed class AppFactory : WebApplicationFactory<Program>
{
    private IHost? _host;

    public AppFactory()
    {
        // Ensure the server is created.
        _ = Server;

        LinkGenerator = _host!.Services.GetRequiredService<LinkGenerator>();
    }

    public string BaseUrl { get; } = $"http://localhost:{GetRandomUnusedPort()}";

    public LinkGenerator LinkGenerator { get; }

    public override ValueTask DisposeAsync()
    {
        _host?.Dispose();
        return base.DisposeAsync();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Staging).ConfigureLogging(b => b.ClearProviders());

        var dummyHost = builder.Build();

#pragma warning disable IDISP003
        _host = builder
            .ConfigureWebHost(b =>
                b.UseSetting("Kestrel:Endpoints:Http:Url", BaseUrl).UseKestrel().UseStaticWebAssets())
            .Build();
#pragma warning restore IDISP003

        _host.Start();

        return dummyHost;
    }

    private static int GetRandomUnusedPort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);

        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();

        return port;
    }
}
