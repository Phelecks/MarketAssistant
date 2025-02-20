using FluentValidation;
using System.Numerics;
using WalletTracker.Application.Interfaces;

namespace WalletTracker.Application.Track.Commands.TrackWallet;

public class TrackWalletCommandValidator : AbstractValidator<TrackWalletCommand>
{
    private readonly IRpcUrlService _rpcUrlService;
    private readonly ITokenService _tokenService;

    public TrackWalletCommandValidator(IRpcUrlService rpcUrlService, ITokenService tokenService)
    {
        _rpcUrlService = rpcUrlService;
        _tokenService = tokenService;

        RuleFor(v => v.rpcUrl)
            .NotEmpty().WithMessage("rpcUrl is required.")
            .NotNull().WithMessage("rpcUrl is required.");
        RuleFor(v => v.account.ChainId)
            .NotEmpty().WithMessage("chainId is required.")
            .NotNull().WithMessage("chainId is required.")
            .Must(BeTokenExists).WithMessage("Main token does not exist.")
            .Must(BePrcUrlExists).WithMessage("Rpc Url does not exist.");
        
    }

    bool BeTokenExists(BigInteger? ChainId)
    {
        if(ChainId is null) return false;
        return _tokenService.GetMainToken((Nethereum.Signer.Chain)(int)ChainId) is not null;
    }

    bool BePrcUrlExists(BigInteger? ChainId)
    {
        if (ChainId is null) return false;
        return _rpcUrlService.GetRpcUrl((Nethereum.Signer.Chain)(int)ChainId) is not null;
    }
}
