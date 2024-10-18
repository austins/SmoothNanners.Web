using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Sockets;

namespace SmoothNanners.Web.Tests.Integration;

internal sealed class AppFactory : WebApplicationFactory<Program>
{
#pragma warning disable CA2213
    private IHost? _host;
#pragma warning restore CA2213

    public AppFactory()
    {
        _ = Server;
        LinkGenerator = _host!.Services.GetRequiredService<LinkGenerator>();
    }

    public string BaseUrl { get; } = $"http://localhost:{GetRandomUnusedPort()}";

    public LinkGenerator LinkGenerator { get; }

    public new void Dispose()
    {
        _host?.Dispose();
        base.Dispose();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Staging).ConfigureLogging(b => b.ClearProviders());

        var dummyHost = builder.Build();

        _host = builder.ConfigureWebHost(b => b.UseSetting("Kestrel:Endpoints:Http:Url", BaseUrl).UseKestrel()).Build();

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
