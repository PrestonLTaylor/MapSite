using Microsoft.AspNetCore.Mvc.Testing;

namespace MapSite.Integration;

internal class IntegrationTest
{
    [OneTimeSetUp]
    protected void Initialization()
    {
        _testClient = _factory.CreateClient();
    }

    protected readonly WebApplicationFactory<Program> _factory = new();
    protected HttpClient _testClient;
}
