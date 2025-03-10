﻿using System.ComponentModel.DataAnnotations;
using BaseApplication.Exceptions;
using BlockChainIdentity.Application.Interfaces;
using BlockChainIdentity.Domain.Events.Wallet;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nethereum.Siwe;
using Nethereum.Siwe.Core;

namespace BlockChainIdentity.Application.Wallet.Commands.AuthenticateWallet;

//[Authorize(roles = "Administrators")]
public record AuthenticateWalletCommand([property: Required] string SiweEncodedMessage, [property: Required] string Signature, [property: Required] int chainId, [property: Required] string ClientKey) : IRequest<TokenDto>;

public class Handler : IRequestHandler<AuthenticateWalletCommand, TokenDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly SiweMessageService _siweMessageService;

    public Handler(IApplicationDbContext context, IIdentityService identityService, SiweMessageService siweMessageService)
    {
        _context = context;
        _identityService = identityService;
        _siweMessageService = siweMessageService;
    }

    public async Task<TokenDto> Handle(AuthenticateWalletCommand request, CancellationToken cancellationToken)
    {
        var client = await GetClientAsync(request.ClientKey, cancellationToken);

        var siweMessage = SiweMessageParser.Parse(request.SiweEncodedMessage);

        var signature = request.Signature;
        var validUser = await _siweMessageService.IsUserAddressRegistered(siweMessage);
        if (validUser)
        {
            if (await _siweMessageService.IsMessageSignatureValid(siweMessage, signature))
            {
                if (_siweMessageService.IsMessageTheSameAsSessionStored(siweMessage))
                {
                    if (_siweMessageService.HasMessageDateStartedAndNotExpired(siweMessage))
                    {
                        var roles = await GetWalletRolesAsync(siweMessage.Address, request.chainId, cancellationToken);
                        var policies = await GetWalletPoliciesAsync(siweMessage.Address, request.chainId, cancellationToken);
                        var tokenResult = await _identityService.GenerateTokenAsync(siweMessage, signature, 
                            roles.Select(s => s.title).ToList(), client.clientResources.Select(s => s.resource.title).ToList(), 
                            policies, client.uri, client.version, request.chainId,
                            siweMessage.RequestId, client.statement, cancellationToken);
                        
                        await CreateTokenAsync(tokenResult.tokenDescriptor.IssuedAt!.Value, tokenResult.tokenDescriptor.Expires!.Value, 
                            tokenResult.tokenDescriptor.NotBefore!.Value, client.statement, client.uri, client.version, siweMessage.Nonce, siweMessage.RequestId,
                            true, siweMessage.Address, request.chainId, client.Id, cancellationToken);

                        return new TokenDto(tokenResult.token, siweMessage.Address);
                    }
                    throw new UnauthorizedAccessException("Message date is not started or expired.");
                }
                throw new UnauthorizedAccessException("Matching Siwe message with nonce not found");
            }
            throw new UnauthorizedAccessException("Invalid Signature");
        }
        throw new UnauthorizedAccessException("Invalid User");
    }


    async Task CreateTokenAsync(DateTime issuedAt, DateTime expireAt, DateTime notBefore, string statement, Uri uri,
     string version, string nonce, string requestId, bool enabled, string walletAddress, int chainId,
     long clientId, CancellationToken cancellationToken)
    {
        var client = await _context.clients.Include(inc => inc.clientResources).ThenInclude(inc => inc.resource).AsNoTracking().SingleAsync(exp => exp.Id == clientId, cancellationToken);

        var wallet = await _context.wallets.Include(inc => inc.walletRoles).ThenInclude(inc => inc.role).SingleOrDefaultAsync(exp => exp.address.Equals(walletAddress), cancellationToken);
        if (wallet == null)
        {
            var defaultRole = await _context.roles.SingleAsync(exp => exp.title.Equals("Users"), cancellationToken);
            wallet = new Domain.Entities.Wallet
            {
                address = walletAddress,
                chainId = chainId,
                walletRoles = new List<Domain.Entities.WalletRole>
                {
                    new Domain.Entities.WalletRole
                    {
                        roleId = defaultRole.Id
                    }
                }
            };

            await _context.wallets.AddAsync(wallet, cancellationToken);
            wallet.AddDomainEvent(new WalletCreatedEvent(wallet, client.clientId));
        }

        var entity = new Domain.Entities.Token
        {
            issuedAt = issuedAt,
            expireAt = expireAt,
            enabled = enabled,
            nonce = nonce,
            notBefore = notBefore,
            requestId = requestId,
            statement = statement,
            version = version,
            walletAddress = walletAddress,
            resources = string.Join(',', client.clientResources.Select(s => s.resource.title).ToList()),
            uri = uri
        };
        await _context.tokens.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    async Task<Domain.Entities.Client> GetClientAsync(string clientKey, CancellationToken cancellationToken)
    {
        var clientResult = _identityService.GetClient(clientKey);
        if (!clientResult.IsSuccess()) throw new ForbiddenAccessException();

        var client = await _context.clients.Include(inc => inc.clientResources).ThenInclude(inc => inc.resource).SingleOrDefaultAsync(exp => exp.clientId.Equals(clientResult.data.ClientId) &&
            exp.clientSecret.Equals(clientResult.data.ClientSecret) && exp.enabled, cancellationToken);
        if (client == null) throw new ForbiddenAccessException();
        return client;
    }

    async Task<List<Domain.Entities.Role>> GetWalletRolesAsync(string address, int chainId, CancellationToken cancellationToken)
    {
        var wallet = await _context.wallets.Include(inc => inc.walletRoles).ThenInclude(inc => inc.role).AsNoTracking().SingleOrDefaultAsync(exp => exp.address.Equals(address) && exp.chainId == chainId, cancellationToken);
        if(wallet == null)
        {
            var defaultRole = await _context.roles.SingleAsync(exp => exp.title.Equals("Users"), cancellationToken);
            return new List<Domain.Entities.Role>
            {
                defaultRole
            };
        }
        return wallet.walletRoles.Select(s => s.role).ToList();
    }

    async Task<List<string>> GetWalletPoliciesAsync(string address, int chainId, CancellationToken cancellationToken)
    {
        return new List<string>();
    }
}
