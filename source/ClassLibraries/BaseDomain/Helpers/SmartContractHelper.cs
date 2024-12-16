using static BaseDomain.Enums.BlockChainEnums;

namespace BaseDomain.Helpers;

public static class SmartContractHelper
{
    internal const string PolygonUsdcContractAddress = "0x2791Bca1f2de4661ED88A30C99A7a9449Aa84174";
    internal const string PolygonUsdtContractAddress = "0xc2132D05D31c914a87C6611C10748AEb04B58e8F";

    public static string GetUsdcContractAddress(int chainId)
    {
        switch (chainId)
        {
            case 137:
                return PolygonUsdcContractAddress;
            default:
                return string.Empty;
        }
    }

    public static string GetUsdtContractAddress(int chainId)
    {
        switch (chainId)
        {
            case 137:
                return PolygonUsdtContractAddress;
            default:
                return string.Empty;
        }
    }
}