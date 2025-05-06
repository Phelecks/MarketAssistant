using BlockChainGasHelper.Helpers;
using System.Text.Json.Serialization;

namespace BlockChainGasHelper.DataTransferObjects;

internal class BaseRequest
{
    [JsonPropertyName("id")]
    public int Id { get; set; } = AlchemyParameters.DefaultId;

    [JsonPropertyName("jsonrpc")]
    public readonly string JsonRpc = AlchemyParameters.DefaultJsonRpc;
}
