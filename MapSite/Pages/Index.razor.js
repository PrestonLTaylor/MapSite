var mapIcons = {};
var entityMarkers = {};
var map;

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

export function AddInitialMapEntities(mapEntities) {
    for (let mapEntityId in mapEntities) {
        let mapEntity = mapEntities[mapEntityId];
        AddMapEntity(mapEntityId, mapEntity);
    }
}