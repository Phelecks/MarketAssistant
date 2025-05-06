using System.Text.Json.Serialization;

namespace BlockChainGasHelper.DataTransferObjects;

internal class BaseResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("jsonrpc")]
    public required string JsonRpc { get; set; }

    [JsonPropertyName("result")]
    public required string Result { get; set; }
}
