namespace LoggerService.Helpers
{
    /// <summary>
    /// System events
    /// </summary>
    public enum EventType
    {
        Information,
        Debug,
        Warning,
        Critical,
        Performance,
        Exception
    }

    /// <summary>
    /// Event tool
    /// </summary>
    public static class EventTool
    {
        /// <summary>
        /// Get event
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static Microsoft.Extensions.Logging.EventId GetEventInformation(EventType eventType, string eventName)
        {
            return new Microsoft.Extensions.Logging.EventId((int)eventType, string.IsNullOrEmpty(eventName) ? eventType.ToString() : eventName);
        }
    }
}
