using MapSite.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace MapSite.Unit;

internal sealed class MapEntityTrackerTest
{
    [Test]
    public void DeleteMapEntity_WhenSuppliedTrackedEntityId_DeletesThatTrackedEntity()
    {
        // Arrange
        const int entityIdToDelete = 0;
        var entityTracker = CreateMapEntityTrackerWithDefaultMapEntities(new Dictionary<int, MapEntity>
        {
            { entityIdToDelete, new MapEntity(new EntityPosition(0, 0), "TestEntity") }
        });

        // Act
        entityTracker.DeleteMapEntity(new EntityDeleteRequest(entityIdToDelete));
        var trackedEntities = entityTracker.GetTrackedEntities();

        // Assert
        Assert.That(trackedEntities.Count(), Is.Zero);
    }

    [Test]
    public void DeleteMapEntity_WhenSuppliedInvalidEntityId_DoesNothing()
    {
        // Arrange
        const int entityIdToDelete = 0;
        const int entityIdToKeep = 1;
        var trackedMapEntity = new MapEntity(new EntityPosition(0, 0), "TestEntity");
        var entityTracker = CreateMapEntityTrackerWithDefaultMapEntities(new Dictionary<int, MapEntity>
        {
            { entityIdToKeep, trackedMapEntity }
        });

        // Act
        entityTracker.DeleteMapEntity(new EntityDeleteRequest(entityIdToDelete));
        var trackedEntities = entityTracker.GetTrackedEntities();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(trackedEntities.Count(), Is.EqualTo(1));
            Assert.That(trackedEntities.First().Key, Is.EqualTo(entityIdToKeep));
            Assert.That(trackedEntities.First().Value, Is.EqualTo(trackedMapEntity));
        });
    }

    private MapEntityTracker CreateMapEntityTrackerWithDefaultMapEntities(Dictionary<int, MapEntity> mapEntities)
    {
        return new MapEntityTracker(new NullLogger<MapEntityTracker>(), mapEntities);
    }
}
