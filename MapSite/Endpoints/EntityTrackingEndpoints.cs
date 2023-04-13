using MapSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace MapSite.Endpoints;

public static class EntityTrackingEndpointsExtensions
{
    static public IServiceCollection AddEntityTrackingServices(this IServiceCollection services)
    {
        services.AddSingleton<MapEntityTracker>();
        return services;
    }

    static public WebApplication MapEntityTrackingEndpoints(this WebApplication app)
    {
        var versioning = app.NewVersionedApi("Entity Tracking");
        var entityTracking = versioning.MapGroup(APIRoutes.EntityTracker.Root).HasApiVersion(APIRoutes.LatestMajorVersion, APIRoutes.LatestMinorVersion);

        entityTracking.MapPost(APIRoutes.EntityTracker.Update, (MapEntityTracker tracker, [FromBody]EntityUpdateRequest updateRequest) => {
            tracker.UpdateTrackedMapEntity(updateRequest);
            return Results.Accepted();
        });

        entityTracking.MapDelete(APIRoutes.EntityTracker.Delete, (MapEntityTracker tracker, int entityId) => {
            var deleted = tracker.TryDeleteMapEntity(entityId);
            return deleted ? Results.NoContent() : Results.NotFound();
        });

        return app;
    }
}
