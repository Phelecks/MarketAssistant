namespace BaseApplication.Exceptions;

public class MaintenanceException : Exception
{
    public MaintenanceException(DateTime dueDateTime)
        : base($"System is in mainenance mode util: {dueDateTime.ToString()} UTC.")
    {
    }
}
