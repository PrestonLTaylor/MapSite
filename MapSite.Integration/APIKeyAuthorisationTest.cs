using MapSite.Endpoints;
using System.Net;

namespace MapSite.Integration;

internal sealed class APIKeyAuthorisationTest : IntegrationTest
{
    [Test]
    public async Task Request_WithoutAPIKey_ReturnsUnauthorised()
    {
        // Act
        var response = await _testClient.GetAsync(APIRoutes.Root);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}
