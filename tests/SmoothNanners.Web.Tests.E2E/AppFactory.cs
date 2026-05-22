using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace SmoothNanners.Web.Tests.E2E;

public sealed class AppFactory : WebApplicationFactory<Program>
{
    public AppFactory()
    {
        UseKestrel(0);
        StartServer();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Staging);
    }
}
