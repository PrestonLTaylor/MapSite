﻿using MapSite.Services;
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
        app.MapPost(APIRoutes.EntityTracker, (MapEntityTracker tracker, [FromBody]EntityUpdateRequest updateRequest) => {
            tracker.UpdateTrackedMapEntity(updateRequest);
            return Results.Accepted();
        });

        app.MapDelete(APIRoutes.EntityTracker, (MapEntityTracker tracker, [FromBody]EntityDeleteRequest deleteRequest) => {
            tracker.DeleteMapEntity(deleteRequest);
            return Results.NoContent();
        });

        return app;
    }
}
