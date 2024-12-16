namespace MassTransitManager.Messages.Interfaces;

public interface IScheduleTriggeredMessage
{
    string ScheduleTitle { get; }
    DateTime TriggeredAt { get; }
}