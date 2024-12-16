namespace BaseApplication.Helpers;

public static class NetworkFeeHelper
{
    public static decimal CalculateNetworkFee(decimal gasPrice, decimal gasUsed)
        => (gasPrice * gasUsed);
}
