using WalletTracker.Application.Interfaces;
using WalletTracker.Domain.Entities;

namespace WalletTracker.Infrastructure.Services;

public class ApplicationInitializer : IApplicationInitializer
{
    private readonly ITokenService _tokenService;
    private readonly IRpcUrlService _rpcUrlService;
    private readonly IAddressService _addressService;

    public ApplicationInitializer(ITokenService tokenService, IRpcUrlService rpcUrlService, IAddressService addressService)
    {
        _tokenService = tokenService;
        _rpcUrlService = rpcUrlService;
        _addressService = addressService;
    }

    public void Initialize(List<Token> tokens, List<RpcUrl> rpcUrls, List<DestinationAddress> destinationAddresses)
    {
        foreach (var token in tokens)
            _tokenService.AddToken(token);

        foreach (var rpcUrl in rpcUrls)
            _rpcUrlService.AddRpcUrl(rpcUrl);
        
        foreach (var destinationAddress in destinationAddresses)
            _addressService.SetDestinationAddress(destinationAddress);
    }
}
