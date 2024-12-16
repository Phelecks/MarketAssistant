using Nethereum.Web3;
using System.Numerics;

namespace BaseDomain.Helpers;

public static class DecimalTools
{
    public static decimal Normalize(this decimal value) => value / 1.000000000000000000000000000000000m;
    
    public static BigInteger ConvertToBigInteger(this decimal value, int decimalPlacesToUnit)
    {
        return Web3.Convert.ToWei(value, decimalPlacesToUnit);
    }

    public static decimal ConvertToDecimal(this BigInteger value, int decimalPlacesToUnit)
    {
        return Web3.Convert.FromWei(value, decimalPlacesToUnit);
    }
}