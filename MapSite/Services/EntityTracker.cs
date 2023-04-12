namespace MapSite.Services;

public sealed class EntityTracker
{
    public EntityTracker(ILogger<EntityTracker> logger)
    {
        _logger = logger;
    }

    public void UpdateTrackedEntity()
    {
        throw new NotImplementedException();
    }

    public void DeleteTrackedEntity()
    {
        throw new NotImplementedException();
    }

    private readonly ILogger<EntityTracker> _logger;
}
