using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class AddNftProductStockMessage : IAddNftProductStockMessage
{
    public AddNftProductStockMessage(string contractAddress, int nftId, int quantity)
    {
        ContractAddress = contractAddress;
        NftId = nftId;
        Quantity = quantity;
    }

    public string ContractAddress { get; }

    public int NftId { get; }

    public int Quantity { get; }
}