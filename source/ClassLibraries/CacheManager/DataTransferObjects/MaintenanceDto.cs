namespace CacheManager.DataTransferObjects;

public class MaintenanceDto
{
    public MaintenanceDto(bool maintenance, DateTime dueDateTime)
    {
        Maintenance = maintenance;
        DueDateTime = dueDateTime;
    }

    public bool Maintenance { get; init; }
    public DateTime DueDateTime { get; init; }
}
