using BlockChainBalanceHelper.Interfaces;
using Nethereum.Web3;
using System.Numerics;

namespace BlockChainBalanceHelper.Services;

public class BalanceService : IBalanceService
{

    public BalanceService()
    {
    }

    public async Task<BigInteger> GetBalanceOfAsync(Web3 web3, string address, CancellationToken cancellationToken = default)
    {
        var result = await web3.Eth.GetBalance.SendRequestAsync(address);
        return result.Value;
    }

    public async Task<BigInteger> GetBalanceOfERC721Async(Web3 web3, string erc721ContractAddress, string address, CancellationToken cancellationToken = default)
    {
        var service = web3.Eth.ERC721.GetContractService(erc721ContractAddress);
        return await service.BalanceOfQueryAsync(address);
    }

    public async Task<BigInteger> GetBalanceOfERC20Async(Web3 web3, string erc20ContractAddress, string address, CancellationToken cancellationToken = default)
    {
        var service = web3.Eth.ERC20.GetContractService(erc20ContractAddress);
        return await service.BalanceOfQueryAsync(address);
    }
}
