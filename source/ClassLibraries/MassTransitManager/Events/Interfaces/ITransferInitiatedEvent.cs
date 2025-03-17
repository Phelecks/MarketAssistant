using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ITransferInitiatedEvent : CorrelatedBy<Guid>
{
    int Chain { get; }
    string Hash { get; }
    string From { get; }
    string To { get; }
    decimal Value { get; }
    DateTime DateTime { get; }
    List<Erc20Transfer>? Erc20Transfers { get; }
    List<Erc721Transfer>? Erc721Transfers { get; }
    
    public record Erc20Transfer(string From, string To, decimal Value, string ContractAddress);
    public record Erc721Transfer(string From, string To, long TokenId, string ContractAddress);
}