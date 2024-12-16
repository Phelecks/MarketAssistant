using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface IAddNftProductStockMessage
{
    string ContractAddress { get; }
    int NftId { get; }
    public int Quantity { get; }
}