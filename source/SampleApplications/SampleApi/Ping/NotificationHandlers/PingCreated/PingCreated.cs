using CustomMediatR.Interfaces;

namespace SampleApi.Ping.NotificationHandlers.PingCreated;

public record PingCreated(string Content) : INotification;