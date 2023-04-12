using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MapSite.Services;

public sealed record APIKey(string Key)
{
    public const string SectionName = "API_KEY";
}

public interface IAPIKeyAuthenticator
{
    public bool IsRequestAuthenticated(HttpRequest request);
}

public sealed class APIKeyAuthenticator : IAPIKeyAuthenticator
{
    public APIKeyAuthenticator(IConfiguration configuration, ILogger<APIKeyAuthenticator> logger)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }
        if (logger is null)
        {
            throw new ArgumentNullException(nameof(logger));
        }

        _configuration = configuration;
        _logger = logger;
    }

    public bool IsRequestAuthenticated(HttpRequest request)
    {
        var validAPIKey = _configuration.GetValue<string>(APIKey.SectionName);
        var requestAPIKey = GetAPIKeyFromRequest(request);
        if (requestAPIKey is null || validAPIKey != requestAPIKey)
        {
            _logger.LogWarning("An invalid key {InvalidAPIKey} was used for API {APIPath}", requestAPIKey, request.Path);
            return false;
        }

        _logger.LogInformation("A valid key {ValidAPIKey} was used for API {APIPath}", requestAPIKey, request.Path);
        return true;
    }

    private string? GetAPIKeyFromRequest(HttpRequest request)
    {
        return request.Headers[APIKey.SectionName];
    }

    private readonly IConfiguration _configuration;
    private readonly ILogger<APIKeyAuthenticator> _logger;
}
