namespace BaseDomain.Helpers;

public static class SmartContractHelper
{
    internal const string PolygonUsdcContractAddress = "0x2791Bca1f2de4661ED88A30C99A7a9449Aa84174";
    internal const string PolygonUsdtContractAddress = "0xc2132D05D31c914a87C6611C10748AEb04B58e8F";

    internal const string EthereumUsdtContractAddress = "0xdac17f958d2ee523a2206206994597c13d831ec7";
    internal const string EthereumUsdcContractAddress = "0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48";

    internal const string BnbUsdtContractAddress = "0x55d398326f99059ff775485246999027b3197955";
    internal const string BnbUsdcContractAddress = "0x8ac76a51cc950d9822d68b83fe1ad97b32cd580d";

    internal const string AvalanchUsdtContractAddress = "0x9702230A8Ea53601f5cD2dc00fDBc13d4dF4A8c7";
    internal const string AvalanchUsdcContractAddress = "0xB97EF9Ef8734C71904D8002F8b6Bc66Dd9c48a6E";

    internal const string OptimismUsdtContractAddress = "0x94b008aa00579c1307b0ef2c499ad98a8ce58e58";
    internal const string OptimismUsdcContractAddress = "0x0b2C639c533813f4Aa9D7837CAf62653d097Ff85";

    internal const string ArbitrumUsdtContractAddress = "0xFd086bC7CD5C481DCC9C85ebE478A1C0b69FCbb9";
    internal const string ArbitrumUsdcContractAddress = "0xaf88d065e77c8cC2239327C5EDb3A432268e5831";

    public static string GetUsdcContractAddress(Nethereum.Signer.Chain chain)
    {
        switch (chain)
        {
            case Nethereum.Signer.Chain.MainNet:
                return EthereumUsdcContractAddress;
            case Nethereum.Signer.Chain.Polygon:
                return PolygonUsdcContractAddress;
            case Nethereum.Signer.Chain.Binance:
                return BnbUsdcContractAddress;
            case Nethereum.Signer.Chain.Avalanche:
                return AvalanchUsdcContractAddress;
            case Nethereum.Signer.Chain.Optimism:
                return OptimismUsdcContractAddress;
            case Nethereum.Signer.Chain.Arbitrum:
                return ArbitrumUsdcContractAddress;
            default:
                return string.Empty;
        }
    }

    public static string GetUsdtContractAddress(Nethereum.Signer.Chain chain)
    {
        switch (chain)
        {
            case Nethereum.Signer.Chain.MainNet:
                return EthereumUsdtContractAddress;
            case Nethereum.Signer.Chain.Polygon:
                return PolygonUsdtContractAddress;
            case Nethereum.Signer.Chain.Binance:
                return BnbUsdtContractAddress;
            case Nethereum.Signer.Chain.Avalanche:
                return AvalanchUsdtContractAddress;
            case Nethereum.Signer.Chain.Optimism:
                return OptimismUsdtContractAddress;
            case Nethereum.Signer.Chain.Arbitrum:
                return ArbitrumUsdtContractAddress;
            default:
                return string.Empty;
        }
    }
}