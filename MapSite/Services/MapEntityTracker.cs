﻿namespace MapSite.Services;

public sealed record EntityPosition(int x, int y);
public sealed record MapEntity(EntityPosition Position, string EntityType);
public sealed record EntityUpdateRequest(int EntityId, MapEntity UpdatedMapEntity);
public sealed record EntityDeleteRequest(int EntityId);

public sealed class MapEntityTracker
{
    public MapEntityTracker(ILogger<MapEntityTracker> logger) : this(logger, new Dictionary<int, MapEntity>()) {}
    public MapEntityTracker(ILogger<MapEntityTracker> logger, Dictionary<int, MapEntity> startingTrackedEntities)
    {
        _entityIdToMapEntity = startingTrackedEntities;
        _logger = logger;
    }

    public void UpdateTrackedMapEntity(EntityUpdateRequest updateRequest)
    {
        _logger.LogInformation("Updated entity with id {Id} and MapEntity {MapEntity}", updateRequest.EntityId, updateRequest.UpdatedMapEntity);

        _entityIdToMapEntity[updateRequest.EntityId] = updateRequest.UpdatedMapEntity;
    }

    public void DeleteMapEntity(EntityDeleteRequest deleteRequest)
    {
        if (_entityIdToMapEntity.Remove(deleteRequest.EntityId))
        {
            _logger.LogInformation("Deleted entity with id: {Id}", deleteRequest.EntityId);
        }
        else
        {
            _logger.LogWarning("Tried to delete an entity that didn't exist with id: {Id}", deleteRequest.EntityId);
        }
    }

    public IEnumerable<KeyValuePair<int, MapEntity>> GetTrackedEntities()
    {
        return _entityIdToMapEntity;
    }

    private readonly Dictionary<int, MapEntity> _entityIdToMapEntity;
    private readonly ILogger<MapEntityTracker> _logger;
}
