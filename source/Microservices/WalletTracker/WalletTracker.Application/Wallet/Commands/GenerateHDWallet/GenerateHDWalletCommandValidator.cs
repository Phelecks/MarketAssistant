using FluentValidation;

namespace WalletTracker.Application.Wallet.Commands.GenerateHDWallet;

public class GenerateHDWalletCommandValidator : AbstractValidator<GenerateHDWalletCommand>
{

    public GenerateHDWalletCommandValidator()
    {

        //RuleFor(v => v.w)
        //    .NotEmpty().WithMessage("rpcUrl is required.")
        //    .NotNull().WithMessage("rpcUrl is required.");

    }
}
