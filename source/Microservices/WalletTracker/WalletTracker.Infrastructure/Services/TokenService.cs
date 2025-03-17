using Nethereum.Signer;
using System.Collections.Concurrent;
using WalletTracker.Application.Interfaces;

namespace WalletTracker.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly ConcurrentBag<Domain.Entities.Token> _tokens = [];

    public void AddToken(Domain.Entities.Token token)
    {
        _tokens.Add(token);
    }

    public List<Domain.Entities.Token> GetAllTokens()
    {
        return _tokens.ToList();
    }

    public List<Domain.Entities.Token> GetErc20Tokens()
    {
        return _tokens.Where(x => x.TokenType == BaseDomain.Enums.BlockChainEnums.TokenType.Erc20).ToList();
    }

    public List<Domain.Entities.Token> GetErc721Tokens()
    {
        return _tokens.Where(x => x.TokenType == BaseDomain.Enums.BlockChainEnums.TokenType.Erc721).ToList();
    }

    public Domain.Entities.Token? GetMainToken(Chain chain)
    {
        return _tokens.FirstOrDefault(x => x.Chain == chain && x.TokenType == BaseDomain.Enums.BlockChainEnums.TokenType.Main);
    }

    public List<Domain.Entities.Token> GetMainTokens()
    {
        return _tokens.Where(x => x.TokenType == BaseDomain.Enums.BlockChainEnums.TokenType.Main).ToList();
    }
}
