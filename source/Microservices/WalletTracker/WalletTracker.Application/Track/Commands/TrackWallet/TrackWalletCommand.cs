using System.ComponentModel.DataAnnotations;
using System.Numerics;
using BaseApplication.Exceptions;
using BlockChainBalanceHelper.Interfaces;
using BlockChainGasHelper.Interfaces;
using BlockChainTransferHelper.Interfaces;
using ExecutorManager.Helpers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Polly;
using WalletTracker.Application.Interfaces;

namespace WalletTracker.Application.Track.Commands.TrackWallet;


public record TrackWalletCommand([property: Required] Nethereum.Web3.Accounts.Account account, [property: Required] Nethereum.Signer.Chain chain, [property: Required] string rpcUrl) : IRequest<Unit>;

public class CreateContactCommandHandler : IRequestHandler<TrackWalletCommand, Unit>
{
    private readonly IBalanceService _balanceService;
    private readonly IGasService _gasService;
    private readonly ITransferService _transferService;
    private readonly ITokenService _tokenService;
    private readonly IAddressService _addressService;
    private readonly ResiliencePipeline _pollyPipeline;

    public CreateContactCommandHandler(IBalanceService balanceService, IGasService gasService, ITransferService transferService,
        ITokenService tokenService, IAddressService addressService,
        [FromKeyedServices(PipelineHelper.RetryEverythingFiveTimes)] ResiliencePipeline pollyPipeline)
    {
        _balanceService = balanceService;
        _gasService = gasService;
        _transferService = transferService;
        _tokenService = tokenService;
        _addressService = addressService;
        _pollyPipeline = pollyPipeline;
    }

    public async Task<Unit> Handle(TrackWalletCommand request, CancellationToken cancellationToken)
    {
        var destinationAddress = _addressService.GetDestinationAddress(request.chain) ?? throw new NotFoundException(nameof(Domain.Entities.DestinationAddress), request.chain);
        
        var web3 = new Web3(request.account, request.rpcUrl);
        var balance = await _pollyPipeline.ExecuteAsync(async ct => await _balanceService.GetBalanceOfAsync(web3, request.account.Address, cancellationToken), cancellationToken);
        
        if(balance > 0)
        {
            BigInteger estimatedGas, gasPrice;

            var erc20Tokens = _tokenService.GetErc20Tokens();
            foreach (var erc20TokenContractAddress in erc20Tokens.Select(s => s.contractAddress))
            {
                if(!string.IsNullOrEmpty(erc20TokenContractAddress))
                {
                    var erc20Balance = await _pollyPipeline.ExecuteAsync(async ct => await _balanceService.GetBalanceOfERC20Async(web3, erc20TokenContractAddress, request.account.Address, cancellationToken), cancellationToken);
                    if (erc20Balance > 0)
                    {
                        estimatedGas = await _pollyPipeline.ExecuteAsync(async ct => await _gasService.EstimateERC20TransferGasAsync(web3, erc20TokenContractAddress, destinationAddress.Address, erc20Balance, cancellationToken), cancellationToken);
                        gasPrice = await _pollyPipeline.ExecuteAsync(async ct => await _gasService.GetGasPriceAsync(request.rpcUrl, cancellationToken), cancellationToken);

                        await _pollyPipeline.ExecuteAsync(async ct => await _transferService.TransferAsync(web3, erc20TokenContractAddress,
                            new Nethereum.Contracts.Standards.ERC20.ContractDefinition.TransferFunction
                            {
                                To = destinationAddress.Address,
                                Value = erc20Balance,
                                Gas = estimatedGas,
                                GasPrice = gasPrice
                            }), cancellationToken);
                    }
                }
            }

            var token = _tokenService.GetMainToken(request.chain) ?? throw new NotFoundException(nameof(Domain.Entities.Token), request.chain);
            
            estimatedGas = await _pollyPipeline.ExecuteAsync(async ct => await _gasService.EstimateTransferGasAsync(web3, destinationAddress.Address, Web3.Convert.FromWei(balance, token.decimals), cancellationToken), cancellationToken);
            gasPrice = await _pollyPipeline.ExecuteAsync(async ct => await _gasService.GetGasPriceAsync(request.rpcUrl, cancellationToken), cancellationToken);
            var transferValue = CalculatedValue(balance, estimatedGas, gasPrice);
            var hash = await _pollyPipeline.ExecuteAsync(async ct => await _transferService.TransferAsync(web3, 
                        new Nethereum.RPC.Eth.DTOs.TransactionInput
                        {
                            To = destinationAddress.Address,
                            Value = transferValue.ToHexBigInteger(),
                            Gas = estimatedGas.ToHexBigInteger(),
                            GasPrice = gasPrice.ToHexBigInteger()
                        }), cancellationToken);
            if(string.IsNullOrEmpty(hash))
            {
                var value = await _pollyPipeline.ExecuteAsync(async ct => await _gasService.CalculateTotalAmountToTransferWholeBalanceInEtherAsync(web3, request.account.Address, cancellationToken), cancellationToken);
                await _pollyPipeline.ExecuteAsync(async ct => await _transferService.TransferAsync(web3, new Nethereum.RPC.Eth.DTOs.TransactionInput
                {
                    To = destinationAddress.Address,
                    Value = Web3.Convert.ToWei(value, token.decimals).ToHexBigInteger(),
                }), cancellationToken);
            }
        }
        return Unit.Value;
    }

    private static BigInteger CalculatedValue(BigInteger balance, BigInteger estimatedGas, BigInteger gasPrice)
    {
        return balance - (estimatedGas * gasPrice);
    }
}
