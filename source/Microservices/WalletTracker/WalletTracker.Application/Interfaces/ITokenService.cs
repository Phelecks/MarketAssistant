namespace WalletTracker.Application.Interfaces;

public interface ITokenService
{
    void AddToken(Domain.Entities.Token token);
    List<Domain.Entities.Token> GetAllTokens();
    List<Domain.Entities.Token> GetMainTokens();
    List<Domain.Entities.Token> GetErc20Tokens();
    List<Domain.Entities.Token> GetErc721Tokens();
    Domain.Entities.Token? GetMainToken(Nethereum.Signer.Chain chain);
}
