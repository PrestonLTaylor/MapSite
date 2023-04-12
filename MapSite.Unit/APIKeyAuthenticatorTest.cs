using MapSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace MapSite.Unit;

// TODO: Use a test data generator for unit tests
internal sealed class APIKeyAuthenticatorTest
{
    [Test]
    public void IsRequestAuthenticated_WhenSuppliedInvalidAPIKey_ReturnsFalse()
    {
        // Arrange
        const string InvalidAPIKey = "INVALID-KEY";
        const string ValidAPIKey = "VALID-KEY";
        var configuration = CreateConfigurationWithAPIKey(ValidAPIKey);
        var request = CreateHttpRequestMockWithAPIKey(InvalidAPIKey);
        var authenticator = CreateAuthenticatorWithConfiguration(configuration);

        // Act
        var isAuthenticated = authenticator.IsRequestAuthenticated(request.Object);

        // Assert
        Assert.That(isAuthenticated, Is.False);
    }

    [Test]
    public void IsRequestAuthenticated_WhenSuppliedNullAPIKey_ReturnsFalse()
    {
        // Arrange
        const string ValidAPIKey = "VALID-KEY";
        var configuration = CreateConfigurationWithAPIKey(ValidAPIKey);
        var request = CreateHttpRequestMockWithAPIKey(null!);
        var authenticator = CreateAuthenticatorWithConfiguration(configuration);

        // Act
        var isAuthenticated = authenticator.IsRequestAuthenticated(request.Object);

        // Assert
        Assert.That(isAuthenticated, Is.False);
    }

    [Test]
    public void IsRequestAuthenticated_WhenSuppliedValidAPIKey_ReturnsTrue()
    {
        // Arrange
        const string ValidAPIKey = "VALID-KEY";
        var configuration = CreateConfigurationWithAPIKey(ValidAPIKey);
        var request = CreateHttpRequestMockWithAPIKey(ValidAPIKey);
        var authenticator = CreateAuthenticatorWithConfiguration(configuration);

        // Act
        var isAuthenticated = authenticator.IsRequestAuthenticated(request.Object);

        // Assert
        Assert.That(isAuthenticated, Is.True);
    }

    private IConfiguration CreateConfigurationWithAPIKey(string apiKey)
    {
        var configurationDictionary = new Dictionary<string, string?>
        {
            { APIKey.SectionName, apiKey }
        };

        return new ConfigurationBuilder().AddInMemoryCollection(configurationDictionary).Build();
    }

    private Mock<HttpRequest> CreateHttpRequestMockWithAPIKey(string apiKey)
    {
        var requestMock = new Mock<HttpRequest>();
        requestMock.SetupGet(x => x.Headers[APIKey.SectionName]).Returns(apiKey);
        return requestMock;
    }

    private APIKeyAuthenticator CreateAuthenticatorWithConfiguration(IConfiguration configuration)
    {
        return new APIKeyAuthenticator(configuration, new NullLogger<APIKeyAuthenticator>());
    }
}
