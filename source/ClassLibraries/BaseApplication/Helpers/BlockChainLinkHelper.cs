namespace BaseApplication.Helpers;

public static class BlockChainLinkHelper
{
    public static string GenerateTransactionUrl(int chainId, string transactionHash)
    {
        return chainId switch
        {
            137 => $"https://polygonscan.com/tx/{transactionHash}",
            8001 => $"https://mumbai.polygonscan.com/tx/{transactionHash}",
            1 => $"https://etherscan.io/tx/{transactionHash}",
            2 => $"https://goerli.etherscan.io/tx/{transactionHash}",
            _ => transactionHash,
        };
    }

    public static string GenerateAddressUrl(int chainId, string address)
    {
        return chainId switch
        {
            137 => $"https://polygonscan.com/address/{address}",
            8001 => $"https://mumbai.polygonscan.com/address/{address}",
            1 => $"https://etherscan.io/address/{address}",
            2 => $"https://goerli.etherscan.io/address/{address}",
            _ => address,
        };
    }
}
