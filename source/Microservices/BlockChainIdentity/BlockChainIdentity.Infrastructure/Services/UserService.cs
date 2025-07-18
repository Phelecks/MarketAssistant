using BlockChainIdentity.Application.Interfaces;
using BlockChainIdentity.Application.Wallet.Queries.GetWallet;
using MediatR.Interfaces;

namespace BlockChainIdentity.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IRequestDispatcher _dispatcher;

    public UserService(IRequestDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public async Task<bool> IsUserAddressRegistered(string address)
    {
        var cancellationToken = new CancellationTokenSource().Token;
        var query = await _dispatcher.SendAsync(new GetWalletQuery(address), cancellationToken);
        return query != null;
    }
}