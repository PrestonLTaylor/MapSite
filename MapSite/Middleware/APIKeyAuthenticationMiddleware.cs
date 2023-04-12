using MapSite.Services;
using System.Net;

namespace MapSite.Middleware;

public sealed class APIKeyAuthenticationMiddleware
{
    public APIKeyAuthenticationMiddleware(RequestDelegate next, IAPIKeyAuthenticator authenticator)
    {
        _next = next;
        _authenticator = authenticator;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/api"))
        {
            await _next.Invoke(context);
            return;
        }

        if (!_authenticator.IsRequestAuthenticated(context.Request))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }

        await _next.Invoke(context);
    }

    private readonly RequestDelegate _next;
    private readonly IAPIKeyAuthenticator _authenticator;
}

public static class APIKeyAuthenticationMiddlewareExtensions
{
    static public IServiceCollection AddAPIKeyAuthenticationServices(this IServiceCollection services)
    {
        services.AddSingleton<IAPIKeyAuthenticator, APIKeyAuthenticator>();

        return services;
    }

    static public IApplicationBuilder UseAPIKeyAuthentication(this IApplicationBuilder app)
    {
        app.UseMiddleware<APIKeyAuthenticationMiddleware>();
        return app;
    }
}
