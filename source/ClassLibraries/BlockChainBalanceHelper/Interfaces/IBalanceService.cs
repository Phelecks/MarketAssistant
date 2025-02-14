using System.Numerics;

namespace BlockChainBalanceHelper.Interfaces;

public interface IBalanceService
{
    /// <summary>
    /// Get main token balance of a chain (like matic in polygon)
    /// </summary>
    /// <param name="address"></param>
    /// <param name="chain"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<BigInteger> GetBalanceOfAsync(string address, Nethereum.Signer.Chain chain, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get balance of an ERC721 contract address like NFT
    /// </summary>
    /// <param name="erc721ContractAddress"></param>
    /// <param name="address"></param>
    /// <param name="chain"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<BigInteger> GetBalanceOfERC721Async(string erc721ContractAddress, string address, Nethereum.Signer.Chain chain, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get balance of an ERC20 contract address like USDC
    /// </summary>
    /// <param name="erc20ContractAddress"></param>
    /// <param name="address"></param>
    /// <param name="chain"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<BigInteger> GetBalanceOfERC20Async(string erc20ContractAddress, string address, Nethereum.Signer.Chain chain, CancellationToken cancellationToken = default);
}