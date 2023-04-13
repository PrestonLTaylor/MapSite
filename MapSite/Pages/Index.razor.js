var mapIcons = {};
var entityMarkers = {};
var map;
var apiKey;

export function InitializeMapIcons(entityTypeToImage) {
    if (!entityTypeToImage) {
        return;
    }

    for (let entityType in entityTypeToImage) {
        mapIcons[entityType] = L.icon({
            iconUrl: entityTypeToImage[entityType],
            iconSize: [64, 64]
        });
    }
}

export function InitializeMapContainer(mapContainerId, mapConfig) {
    map = L.map(mapContainerId, {
        crs: L.CRS.Simple,
        minZoom: -5
    });

    const bounds = [[mapConfig.topX, mapConfig.topY], [mapConfig.bottomX, mapConfig.bottomX]];
    L.imageOverlay(mapConfig.imageOverlay, bounds).addTo(map);
    map.fitBounds(bounds);
}

function AddMapEntity(mapEntityId, mapEntity) {
    let icon = mapIcons[mapEntity.entityType];
    if (icon) {
        entityMarkers[mapEntityId] = L.marker([mapEntity.position.x, mapEntity.position.y], { icon: mapIcons[mapEntity.entityType]}).addTo(map);
    } else {
        entityMarkers[mapEntityId] = L.marker([mapEntity.position.x, mapEntity.position.y]).addTo(map);
    }
}

function UpdateMapMarkersWithEntityJson(json) {
    for (var entityId in json) {
        var entity = json[entityId];
        var marker = entityMarkers[entityId];
        if (marker) {
            marker.setLatLng([entity.position.x, entity.position.y]);
        }
        else {
            AddMapEntity(entityId, entity);
        }
    }

    // Check if an entity was removed
    for (var entityId in entityMarkers) {
        var entity = json[entityId]
        if (!entity) {
            var marker = entityMarkers[entityId];
            map.removeLayer(marker);
        }
    }
}

function MapEntityPolling() {
    fetch("/api/entitytracker",
        {
            headers: { "API_KEY": apiKey }
        })
        .then((response) => response.json())
        .then((json) => UpdateMapMarkersWithEntityJson(json));
}

export function InitializePolling(providedApiKey) {
    apiKey = providedApiKey;
    setInterval(MapEntityPolling, 1000);
}