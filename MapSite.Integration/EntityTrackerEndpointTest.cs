using MapSite.Endpoints;
using MapSite.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace MapSite.Integration;

internal sealed class EntityTrackerEndpointTest : IntegrationTest
{
    [OneTimeSetUp]
    new public void Initialization()
    {
        AddValidAPIKeyToTestClient();
    }

    [Test]
    public async Task Post_WhenSuppliedNewEntity_AddsEntityToEntityTracker()
    {
        // Arrange
        const string apiPath = APIRoutes.EntityTracker.Root + APIRoutes.EntityTracker.Update;
        const int uniqueEntityId = 0;
        var uniqueMapEntity = new MapEntity(new EntityPosition(0, 0), "TestEntity");
        var entityUpdateRequest = new EntityUpdateRequest(uniqueEntityId, uniqueMapEntity);

        // Act
        var response = await _testClient.PostAsJsonAsync(apiPath, entityUpdateRequest);
        var entityTracker = _factory.Services.GetRequiredService<MapEntityTracker>();
        var trackedEntities = entityTracker.GetTrackedEntities();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));
            Assert.That(trackedEntities.Count(), Is.EqualTo(1));
            Assert.That(trackedEntities.First().Key, Is.EqualTo(uniqueEntityId));
            Assert.That(trackedEntities.First().Value, Is.EqualTo(uniqueMapEntity));
        });
    }

    [Test]
    public async Task Post_WhenSuppliedExistingEntity_UpdatesThatEntityInEntityTracker()
    {
        // Arrange
        const string apiPath = APIRoutes.EntityTracker.Root + APIRoutes.EntityTracker.Update;
        const int uniqueEntityId = 0;
        var originalMapEntity = new MapEntity(new EntityPosition(0, 0), "TestEntity");
        var newMapEntity = new MapEntity(new EntityPosition(1, 1), "TestEntity");
        var originalEntityUpdateRequest = new EntityUpdateRequest(uniqueEntityId, originalMapEntity);
        var newEntityUpdateRequest = new EntityUpdateRequest(uniqueEntityId, newMapEntity);

        // Act
        await _testClient.PostAsJsonAsync(apiPath, originalEntityUpdateRequest);
        var response = await _testClient.PostAsJsonAsync(apiPath, newEntityUpdateRequest);
        var entityTracker = _factory.Services.GetRequiredService<MapEntityTracker>();
        var trackedEntities = entityTracker.GetTrackedEntities();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));
            Assert.That(trackedEntities.Count(), Is.EqualTo(1));
            Assert.That(trackedEntities.First().Key, Is.EqualTo(uniqueEntityId));
            Assert.That(trackedEntities.First().Value, Is.EqualTo(newMapEntity));
        });
    }

    [Test]
    public async Task Delete_WhenSuppliedExistingEntity_RemovesEntityFromEntityTracker()
    {
        // Arrange
        const string updatePath = APIRoutes.EntityTracker.Root + APIRoutes.EntityTracker.Update;
        const int uniqueEntityId = 0;
        var uniqueMapEntity = new MapEntity(new EntityPosition(0, 0), "TestEntity");
        var entityUpdateRequest = new EntityUpdateRequest(uniqueEntityId, uniqueMapEntity);

        // Act
        await _testClient.PostAsJsonAsync(updatePath, entityUpdateRequest);
        var response = await _testClient.DeleteAsync($"{APIRoutes.EntityTracker.Root}/{uniqueEntityId}");
        var entityTracker = _factory.Services.GetRequiredService<MapEntityTracker>();
        var trackedEntities = entityTracker.GetTrackedEntities();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(trackedEntities.Count(), Is.EqualTo(0));
        });
    }

    [Test]
    public async Task Delete_WhenSuppliedInvalidEntity_DoesNothing()
    {
        // Arrange
        const string updatePath = APIRoutes.EntityTracker.Root + APIRoutes.EntityTracker.Update;
        const int uniqueEntityId = 0;
        const int invalidEntityId = 1;
        var uniqueMapEntity = new MapEntity(new EntityPosition(0, 0), "TestEntity");
        var entityUpdateRequest = new EntityUpdateRequest(uniqueEntityId, uniqueMapEntity);

        // Act
        await _testClient.PostAsJsonAsync(updatePath, entityUpdateRequest);
        var response = await _testClient.DeleteAsync($"{APIRoutes.EntityTracker.Root}/{invalidEntityId}");
        var entityTracker = _factory.Services.GetRequiredService<MapEntityTracker>();
        var trackedEntities = entityTracker.GetTrackedEntities();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(trackedEntities.Count(), Is.EqualTo(1));
        });
    }

    private void AddValidAPIKeyToTestClient()
    {
        var configuration = _factory.Services.GetRequiredService<IConfiguration>();
        _testClient.DefaultRequestHeaders.Add(APIKey.SectionName, configuration.GetValue<string>(APIKey.SectionName));
    }
}
