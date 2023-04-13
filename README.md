# MapSite

MapSite is an ASP.NET Web and API application that allows you to have a map with entities updated in real-time using the API.

# Startup
To start the application, run this command in the root folder:
```
dotnet run --project ./MapSite/MapSite.csproj
```

# Documentation

To get all tracked entities:
```
GET /api/entitytracker
```

To add a tracked entity:
```
POST /api/entitytracker

Body:
{
    "EntityId": 0,
    "UpdatedMapEntity": {
        "Position": {
            "x": 1000,
            "y": 2000
        },
        "EntityType": "Marker"
    }
}
```

To delete a tracked entity:
```
DELETE /api/entitytracker/{entityId}
```

# Testing

To run the tests for the application, run this command in the root folder:
```
dotnet test
```
