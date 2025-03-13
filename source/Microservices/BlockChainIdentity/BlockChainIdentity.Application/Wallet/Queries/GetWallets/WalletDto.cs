using BaseApplication.Mappings;

namespace BlockChainIdentity.Application.Wallet.Queries.GetWallets;

public class WalletDto : IMapFrom<Domain.Entities.Wallet>
{
    public required string Address { get; set; }

    public int ChainId { get; set; }

    public DateTime Created { get; set; }
}
