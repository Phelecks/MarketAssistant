namespace Informing.Application.Interfaces;

public interface IFCMService
{
    Task SendMessagePushNotificationAsync(SendMessagePushNotificationDto dto, CancellationToken cancellationToken);
}
public record SendMessagePushNotificationDto(string deviceToken, string conversationTitle, string message);