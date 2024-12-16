using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class DeleteCollectionMessage : IDeleteCollectionMessage
{
    public DeleteCollectionMessage(Guid correlationId, string title)
    {
        CorrelationId = correlationId;
        Title = title;
    }

    public Guid CorrelationId { get; }
    public string Title { get; }
}