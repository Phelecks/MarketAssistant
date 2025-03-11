namespace BlockChainGasHelper.DataTransferObjects;

internal class BaseResponse
{
    public int id { get; set; }
    public required string jsonrpc { get; set; }
    public required string result { get; set; }
}
