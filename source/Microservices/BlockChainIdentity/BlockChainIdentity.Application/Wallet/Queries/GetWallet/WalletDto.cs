using BaseApplication.Mappings;

namespace BlockChainIdentity.Application.Wallet.Queries.GetWallet;

public class WalletDto : IMapFrom<Domain.Entities.Wallet>
{
    public string address { get; set; }

    public int chainId { get; set; }
}
