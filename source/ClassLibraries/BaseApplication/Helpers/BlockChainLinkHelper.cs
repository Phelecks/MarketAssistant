namespace BaseApplication.Helpers;

public static class BlockChainLinkHelper
{
    public static string GenerateTransactionUrl(int chainId, string transactionHash)
    {
        switch (chainId)
        {
            case 137:
                return $"https://polygonscan.com/tx/{transactionHash}";
            case 8001:
                return $"https://mumbai.polygonscan.com/tx/{transactionHash}";
            case 1:
                return $"https://etherscan.io/tx/{transactionHash}";
            case 2:
                return $"https://goerli.etherscan.io/tx/{transactionHash}";
            default:
                return transactionHash;
        }
    }

    public static string GenerateAddressUrl(int chainId, string address)
    {
        switch (chainId)
        {
            case 137:
                return $"https://polygonscan.com/address/{address}";
            case 8001:
                return $"https://mumbai.polygonscan.com/address/{address}";
            case 1:
                return $"https://etherscan.io/address/{address}";
            case 2:
                return $"https://goerli.etherscan.io/address/{address}";
            default:
                return address;
        }
    }
}
