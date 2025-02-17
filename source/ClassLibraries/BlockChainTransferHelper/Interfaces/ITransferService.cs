using Nethereum.Web3;

namespace BlockChainTransferHelper.Interfaces;

public interface ITransferService
{
    Task<string> TransferAsync(Web3 web3, Nethereum.RPC.Eth.DTOs.TransactionInput request);
    Task<string> TransferAsync(Web3 web3, string contractAddress,
        Nethereum.Contracts.Standards.ERC20.ContractDefinition.TransferFunction request);
    Task<Nethereum.RPC.Eth.DTOs.TransactionReceipt?> TransferAndWaitForReceiptAsync(Web3 web3, Nethereum.RPC.Eth.DTOs.TransactionInput request);
    Task<Nethereum.RPC.Eth.DTOs.TransactionReceipt?> TransferAndWaitForReceiptAsync(Web3 web3, string contractAddress,
        Nethereum.Contracts.Standards.ERC20.ContractDefinition.TransferFunction request);
}
