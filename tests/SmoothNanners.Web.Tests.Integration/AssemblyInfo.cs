using SmoothNanners.Web.Tests.Integration;
using Xunit.v3;

[assembly: TestPipelineStartup(typeof(AssemblySetup))]
[assembly: AssemblyFixture(typeof(TestFixture))]
