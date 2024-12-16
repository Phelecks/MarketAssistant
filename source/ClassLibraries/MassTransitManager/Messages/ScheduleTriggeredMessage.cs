using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class ScheduleTriggeredMessage : IScheduleTriggeredMessage
{
    public ScheduleTriggeredMessage(string scheduleTitle, DateTime triggeredAt)
    {
        ScheduleTitle = scheduleTitle;
        TriggeredAt = triggeredAt;
    }

    public string ScheduleTitle { get; }

    public DateTime TriggeredAt { get; }
}