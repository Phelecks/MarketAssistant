using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class DeleteNftTokenMessage : IDeleteNftTokenMessage
{
    public DeleteNftTokenMessage(Guid correlationId, string contractAddress)
    {
        CorrelationId = correlationId;
        ContractAddress = contractAddress;
    }

    public Guid CorrelationId { get; }
    public string ContractAddress { get; }
}