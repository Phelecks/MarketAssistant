using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface IChangeNftPriceMessage : CorrelatedBy<Guid>
{
    string ContractAddress { get; }
    int NftId { get; }
    decimal NewPrice { get; }
}