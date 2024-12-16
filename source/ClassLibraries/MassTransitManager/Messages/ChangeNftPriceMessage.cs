using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class ChangeNftPriceMessage : IChangeNftPriceMessage
{
    public ChangeNftPriceMessage(Guid correlationId, string contractAddress, int nftId, decimal newPrice)
    {
        ContractAddress = contractAddress;
        NftId = nftId;
        NewPrice = newPrice;
        CorrelationId = correlationId;
    }

    public string ContractAddress { get; }

    public int NftId { get; }

    public decimal NewPrice { get; }

    public Guid CorrelationId { get; }
}