using BlockChainIdentity.Application.Interfaces;
using BlockChainIdentity.Application.Wallet.Queries.GetWallet;
using MediatR;

namespace BlockChainIdentity.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly ISender _sender;

    public UserService(ISender sender)
    {
        _sender = sender;
    }

    public async Task<bool> IsUserAddressRegistered(string address)
    {
        var query = await _sender.Send(new GetWalletQuery(address));
        return query != null;
    }
}