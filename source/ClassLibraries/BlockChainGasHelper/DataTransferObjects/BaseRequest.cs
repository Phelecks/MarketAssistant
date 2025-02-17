using BlockChainGasHelper.Helpers;
using System.Text.Json.Serialization;

namespace BlockChainGasHelper.DataTransferObjects;

internal class BaseRequest
{
    [JsonPropertyName("id")]
    public int id { get; set; } = AlchemyParameters.DefaultId;
    [JsonPropertyName("jsonrpc")]
    public readonly string jsonrpc = AlchemyParameters.DefaultJsonRpc;
}
