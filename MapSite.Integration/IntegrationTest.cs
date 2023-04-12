using MapSite.Endpoints;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace MapSite.Integration;

internal class IntegrationTest
{
    protected IntegrationTest()
    {
        var factory = new WebApplicationFactory<Program>();
        _testClient = factory.CreateClient();
    }

    protected readonly HttpClient _testClient;
}
