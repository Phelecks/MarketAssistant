using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface ICreateCloseMatchWalletDocumentMessage
{
    Guid TransferId { get; }
    string TransactionHash { get; }
    string From { get; }
    string To { get; }
    long ExternalTokenId { get; }
    decimal Value { get; }
    DateTime DateTime { get; }
    NetworkFeeDto NetworkFee { get; }

    record NetworkFeeDto(long ExternalTokenId, decimal Value);
}