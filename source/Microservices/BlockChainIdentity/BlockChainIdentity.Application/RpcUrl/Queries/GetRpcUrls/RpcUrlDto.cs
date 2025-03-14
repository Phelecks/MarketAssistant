using BaseApplication.Mappings;

namespace BlockChainIdentity.Application.RpcUrl.Queries.GetRpcUrls;

public class RpcUrlDto : IMapFrom<Domain.Entities.RpcUrl>
{
    public Nethereum.Signer.Chain Chain { get; set; }
    public required Uri Uri { get; set; }
    public int BlockOfConfirmation { get; set; }    
    public int WaitIntervalOfBlockProgress { get; set; }
}
