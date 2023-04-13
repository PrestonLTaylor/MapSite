using MapSite.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace MapSite.Unit;

internal sealed class MapEntityTrackerTest
{
    [Test]
    public void UpdateMapEntity_WhenSuppliedNewEntityId_CreatesNewTrackedEntity()
    {
        // Arrange
        const int uniqueEntityId = 0;
        var trackedMapEntity = new MapEntity(new EntityPosition(0, 0), "TestEntity");
        var entityTracker = CreateMapEntityTrackerWithDefaultMapEntities(new Dictionary<int, MapEntity>
        {
            { uniqueEntityId, new MapEntity(new EntityPosition(0, 0), "TestEntity") }
        });

        // Act
        entityTracker.UpdateTrackedMapEntity(new EntityUpdateRequest(uniqueEntityId, trackedMapEntity));
        var trackedEntities = entityTracker.GetTrackedEntities();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(trackedEntities.Count(), Is.EqualTo(1));
            Assert.That(trackedEntities.First().Key, Is.EqualTo(uniqueEntityId));
            Assert.That(trackedEntities.First().Value, Is.EqualTo(trackedMapEntity));
        });
    }

    [Test]
    public void UpdateMapEntity_WhenSuppliedExistingEntityId_UpdatesTrackedEntity()
    {
        // Arrange
        const int uniqueEntityId = 0;
        var originalMapEntity = new MapEntity(new EntityPosition(0, 0), "TestEntity");
        var updatedMapEntity = new MapEntity(new EntityPosition(1, 1), "TestEntity");
        var entityTracker = CreateMapEntityTrackerWithDefaultMapEntities(new Dictionary<int, MapEntity>
        {
            { uniqueEntityId, originalMapEntity }
        });

        // Act
        entityTracker.UpdateTrackedMapEntity(new EntityUpdateRequest(uniqueEntityId, updatedMapEntity));
        var trackedEntities = entityTracker.GetTrackedEntities();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(trackedEntities.Count(), Is.EqualTo(1));
            Assert.That(trackedEntities.First().Key, Is.EqualTo(uniqueEntityId));
            Assert.That(trackedEntities.First().Value, Is.EqualTo(updatedMapEntity));
        });
    }

    [Test]
    public void TryDeleteMapEntity_WhenSuppliedTrackedEntityId_DeletesThatTrackedEntity()
    {
        // Arrange
        const int entityIdToDelete = 0;
        var entityTracker = CreateMapEntityTrackerWithDefaultMapEntities(new Dictionary<int, MapEntity>
        {
            { entityIdToDelete, new MapEntity(new EntityPosition(0, 0), "TestEntity") }
        });

        // Act
        var deleted = entityTracker.TryDeleteMapEntity(entityIdToDelete);
        var trackedEntities = entityTracker.GetTrackedEntities();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(deleted, Is.True);
            Assert.That(trackedEntities.Count(), Is.Zero);
        });
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
        var deleted = entityTracker.TryDeleteMapEntity(entityIdToDelete);
        var trackedEntities = entityTracker.GetTrackedEntities();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(deleted, Is.False);
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
