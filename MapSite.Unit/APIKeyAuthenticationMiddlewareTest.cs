using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MapSite.Middleware;
using MapSite.Services;
using System.Net;

namespace MapSite.Unit;

internal sealed class APIKeyAuthenticationMiddlewareTest
{
    [Test]
    public async Task Middleware_ReturnsUnauthorised_WhenRequestHasNoAPIKeyHeader()
    {
        // Arrange
        const string APIPath = "/api";
        const string ValidAPIKey = "VALID-KEY";
        using var host = await StartHostWithMiddlewareWithAPIKeyAsync(ValidAPIKey);

        // Act
        var response = await host.GetTestClient().GetAsync(APIPath);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task Middleware_ReturnsUnauthorised_WhenRequestHasInvalidAPIKey()
    {
        // Arrange
        const string APIPath = "/api";
        const string ValidAPIKey = "VALID-KEY";
        const string InvalidAPIKey = "INVALID-KEY";
        using var host = await StartHostWithMiddlewareWithAPIKeyAsync(ValidAPIKey);
        var testClient = SetupTestClientToHaveAPIKey(host, InvalidAPIKey);

        // Act
        var response = await testClient.GetAsync(APIPath);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task Middleware_ReturnsNotFound_WhenRequestHasValidAPIKey()
    {
        // Arrange
        const string APIPath = "/api";
        const string ValidAPIKey = "VALID-KEY";
        using var host = await StartHostWithMiddlewareWithAPIKeyAsync(ValidAPIKey);
        var testClient = SetupTestClientToHaveAPIKey(host, ValidAPIKey);
        // Act
        var response = await testClient.GetAsync(APIPath);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Middleware_ReturnsNotFound_WhenRequestIsNotForAPI()
    {
        // Arrange
        const string Path = "/";
        const string ValidAPIKey = "VALID-KEY";
        using var host = await StartHostWithMiddlewareWithAPIKeyAsync(ValidAPIKey);

        // Act
        var response = await host.GetTestClient().GetAsync(Path);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    private async Task<IHost> StartHostWithMiddlewareWithAPIKeyAsync(string apiKey)
    {
        return await new HostBuilder().ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseTestServer()
                    .ConfigureAppConfiguration((config) =>
                    {
                        var configDictionary = new Dictionary<string, string?>
                        {
                            { APIKey.SectionName, apiKey }
                        };

                        config.AddInMemoryCollection(configDictionary);
                    })
                    .ConfigureServices((services) =>
                    {
                        services.AddAPIKeyAuthenticationServices();
                    })
                    .Configure((app) =>
                    {
                        app.UseAPIKeyAuthentication();
                    });
            }).StartAsync();
    }

    private HttpClient SetupTestClientToHaveAPIKey(IHost host, string apiKey)
    {
        var testClient = host.GetTestClient();
        testClient.DefaultRequestHeaders.Add(APIKey.SectionName, apiKey);
        return testClient;
    }
}
