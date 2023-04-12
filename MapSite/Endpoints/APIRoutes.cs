namespace MapSite.Endpoints;

public struct APIRoutes
{
    public const string Root = "/api";
    public struct EntityTracker
    {
        public const string Root = APIRoutes.Root + "/entitytracker";
        public const string Update = Root;
        public const string Delete = Root + "/{entityId}";
    }
}
