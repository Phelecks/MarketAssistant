using System.Text.Json.Serialization;

namespace BlockChainGasHelper.DataTransferObjects;

internal class GasPriceRequest : BaseRequest
{
    [JsonPropertyName("method")]
    internal readonly string Method = "eth_gasPrice";
}
