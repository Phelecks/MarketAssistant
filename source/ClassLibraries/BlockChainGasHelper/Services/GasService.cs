﻿using BlockChainGasHelper.DataTransferObjects;
using BlockChainGasHelper.Interfaces;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Fee1559Suggestions;
using Nethereum.Web3;
using System.Net.Http.Json;
using System.Numerics;

namespace BlockChainGasHelper.Services;

public class GasService : IGasService
{
    private readonly HttpClient _httpClient;

    public GasService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BigInteger> GetGasPriceAsync(string rpcUrl, CancellationToken cancellationToken)
    {
        var requestDto = new GasPriceRequest();
        var request = new HttpRequestMessage(HttpMethod.Post, rpcUrl);
        request.Headers.Add("accept", "application/json");
        var content = new StringContent($"{{\"id\": {requestDto.Id},\"jsonrpc\": \"{requestDto.JsonRpc}\",\"method\": \"{requestDto.Method}\"}}", null, "application/json");
        request.Content = content;
        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<BaseResponse>(cancellationToken) ?? throw new InvalidOperationException("Cannot parse alchemy response.");
        return new HexBigInteger(result.Result).Value;
    }

    public async Task<BigInteger> EstimateTransferGasAsync(Web3 web3, string toAddress, decimal value, CancellationToken cancellationToken = default)
    {
        return await web3.Eth.GetEtherTransferService().EstimateGasAsync(toAddress, value);
    }

    public async Task<BigInteger> EstimateERC721TransferGasAsync(Web3 web3, string erc721ContractAddress, string toAddress, int tokenId, CancellationToken cancellationToken = default)
    {
        var transferHandler = web3.Eth.GetContractTransactionHandler<Nethereum.Contracts.Standards.ERC721.ContractDefinition.TransferFromFunction>();
        var transfer = new Nethereum.Contracts.Standards.ERC721.ContractDefinition.TransferFromFunction()
        {
            To = toAddress,
            TokenId = tokenId,
        };
        var estimate = await transferHandler.EstimateGasAsync(erc721ContractAddress, transfer);

        return estimate.Value;
    }

    public async Task<BigInteger> EstimateERC20TransferGasAsync(Web3 web3, string erc20ContractAddress, string toAddress, BigInteger value, CancellationToken cancellationToken = default)
    {
        var transferHandler = web3.Eth.GetContractTransactionHandler<Nethereum.Contracts.Standards.ERC20.ContractDefinition.TransferFunction>();
        var transfer = new Nethereum.Contracts.Standards.ERC20.ContractDefinition.TransferFunction()
        {
            To = toAddress,
            AmountToSend = value
        };
        var estimate = await transferHandler.EstimateGasAsync(erc20ContractAddress, transfer);

        return estimate.Value;
    }

    public async Task<Fee1559> SuggestFeeToTransferWholeBalanceInEtherAsync(Web3 web3, string fromAddress, CancellationToken cancellationToken = default)
    {
        return await web3.Eth.GetEtherTransferService().SuggestFeeToTransferWholeBalanceInEtherAsync();
    }

    public async Task<decimal> CalculateTotalAmountToTransferWholeBalanceInEtherAsync(Web3 web3, string fromAddress, CancellationToken cancellationToken = default)
    {
        var suggestedFee = await web3.Eth.GetEtherTransferService().SuggestFeeToTransferWholeBalanceInEtherAsync();
        if (suggestedFee is null || suggestedFee.MaxFeePerGas is null) throw new InvalidOperationException("Suggested fee or MaxFeePerGas is null.");
        return await web3.Eth.GetEtherTransferService().CalculateTotalAmountToTransferWholeBalanceInEtherAsync(address: fromAddress, maxFeePerGas: suggestedFee.MaxFeePerGas.Value);
    }
}
