using MapSite.Services;

namespace MapSite.Endpoints;

public static class EntityTrackingEndpointsExtensions
{
    static public IServiceCollection AddEntityTrackingServices(this IServiceCollection services)
    {
        services.AddSingleton<EntityTracker>();
        return services;
    }

    static public WebApplication MapEntityTrackingEndpoints(this WebApplication app)
    {
        app.MapPost(APIRoutes.EntityTracker, (EntityTracker tracker) => {
            tracker.UpdateTrackedEntity();
            return Results.NoContent();
        });

        app.MapDelete(APIRoutes.EntityTracker, (EntityTracker tracker) => {
            tracker.DeleteTrackedEntity();
            return Results.NoContent();
        });

        return app;
    }
}
