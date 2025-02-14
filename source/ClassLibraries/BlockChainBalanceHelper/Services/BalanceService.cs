using BlockChainBalanceHelper.Interfaces;
using BlockChainWeb3ProviderHelper.Interfaces;
using System.Numerics;

namespace BlockChainBalanceHelper.Services;

public class BalanceService : IBalanceService
{
    private readonly IWeb3ProviderService _web3ProviderService;

    public BalanceService(IWeb3ProviderService web3ProviderService)
    {
        _web3ProviderService = web3ProviderService;
    }

    public async Task<BigInteger> GetBalanceOfAsync(string address, Nethereum.Signer.Chain chain, CancellationToken cancellationToken = default)
    {
        var web3 = _web3ProviderService.CreateWeb3(chain, cancellationToken);
        var result = await web3.Eth.GetBalance.SendRequestAsync(address);
        return result.Value;
    }

    public async Task<BigInteger> GetBalanceOfERC721Async(string erc721ContractAddress, string address, Nethereum.Signer.Chain chain, CancellationToken cancellationToken = default)
    {
        var web3 = _web3ProviderService.CreateWeb3(chain, cancellationToken);
        var service = web3.Eth.ERC721.GetContractService(erc721ContractAddress);
        return await service.BalanceOfQueryAsync(address);
    }

    public async Task<BigInteger> GetBalanceOfERC20Async(string erc20ContractAddress, string address, Nethereum.Signer.Chain chain, CancellationToken cancellationToken = default)
    {
        var web3 = _web3ProviderService.CreateWeb3(chain, cancellationToken);
        var service = web3.Eth.ERC20.GetContractService(erc20ContractAddress);
        return await service.BalanceOfQueryAsync(address);
    }
}
