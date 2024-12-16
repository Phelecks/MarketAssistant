using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.Wallet.Commands.AuthenticateWallet;

public record TokenDto([property: Required] string Token, [property: Required] string Address);