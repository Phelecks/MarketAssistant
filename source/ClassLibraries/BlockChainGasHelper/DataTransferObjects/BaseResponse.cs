namespace BlockChainGasHelper.DataTransferObjects;

internal class BaseResponse
{
    public int id { get; set; }
    public string jsonrpc { get; set; }
    public string result { get; set; }
}
