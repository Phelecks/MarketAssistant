using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class FindParentOfCustomerMessage : IFindParentOfCustomerMessage
{
    public FindParentOfCustomerMessage(Guid correlationId, string userId)
    {
        CorrelationId = correlationId;
        UserId = userId;
    }

    public Guid CorrelationId { get; }
    public string UserId { get; }
}