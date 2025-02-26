using FluentValidation;
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
        RuleFor(v => v.chain)
            .NotEmpty().WithMessage("chainId is required.")
            .NotNull().WithMessage("chainId is required.")
            .Must(BeTokenExists).WithMessage("Main token does not exist.")
            .Must(BePrcUrlExists).WithMessage("Rpc Url does not exist.");
        
    }

    bool BeTokenExists(Nethereum.Signer.Chain Chain)
    {
        return _tokenService.GetMainToken(Chain) is not null;
    }

    bool BePrcUrlExists(Nethereum.Signer.Chain Chain)
    {
        return _rpcUrlService.GetRpcUrl(Chain) is not null;
    }
}
