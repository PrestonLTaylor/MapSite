namespace MapSite.Endpoints;

public struct APIRoutes
{
    public const string Root = "/api";

    public const int LatestMajorVersion = 1;
    public const int LatestMinorVersion = 0;

    public struct EntityTracker
    {
        public const string Root = APIRoutes.Root + "/entitytracker";
        public const string Update = "/";
        public const string Delete = "/{entityId}";
    }
}
