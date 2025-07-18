using BlockChain.Application.Interfaces;
using BlockChain.Domain.Events.Notification;
using MassTransitManager.Messages.Interfaces;
using MediatR.Helpers;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlockChain.Application.Customer.Commands.ProcessTransferConfirmed;

public record ProcessTransferConfirmedCommand([property: Required] ITransferConfirmedMessage TransferConfirmedEvent) 
    : IRequest<Unit>;

public class Handler(IApplicationDbContext context) : IRequestHandler<ProcessTransferConfirmedCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> HandleAsync(ProcessTransferConfirmedCommand request, CancellationToken cancellationToken)
    {
        var fromTracks = await _context.Tracks.Where(exp => exp.WalletAddress.Equals(request.TransferConfirmedEvent.From)).ToListAsync(cancellationToken);
        foreach(var item in fromTracks)
        {
            item.AddDomainNotification(new NotificationCreatedEvent(
                correlationId: request.TransferConfirmedEvent.CorrelationId, 
                walletAddress: item.CustomerWalletAddress, 
                transfer: new NotificationCreatedEvent.Transfer(
                    Chain: request.TransferConfirmedEvent.Chain, 
                    Hash: request.TransferConfirmedEvent.Hash, 
                    From: request.TransferConfirmedEvent.From,
                    To: request.TransferConfirmedEvent.To, 
                    Value: request.TransferConfirmedEvent.Value, 
                    DateTime: request.TransferConfirmedEvent.DateTime,
                    Erc20Transfers: request.TransferConfirmedEvent.Erc20Transfers?.Select(s => new NotificationCreatedEvent.Erc20Transfer(s.From, s.To, s.Value, s.ContractAddress)).ToList(),
                    Erc721Transfers: request.TransferConfirmedEvent.Erc721Transfers?.Select(s => new NotificationCreatedEvent.Erc721Transfer(s.From, s.To, s.TokenId, s.ContractAddress)).ToList())));
        }

        var destinationAddresses = GetDestinationAddresses(request.TransferConfirmedEvent);
        if(destinationAddresses is not null)
            foreach(var destinationAddress in destinationAddresses)
            {
                var toTracks = await _context.Tracks.Where(exp => exp.WalletAddress.Equals(request.TransferConfirmedEvent.From)).ToListAsync(cancellationToken);
                foreach(var item in toTracks)
                {
                    item.AddDomainNotification(new NotificationCreatedEvent(
                        correlationId: request.TransferConfirmedEvent.CorrelationId, 
                        walletAddress: item.CustomerWalletAddress, 
                        transfer: new NotificationCreatedEvent.Transfer(
                            Chain: request.TransferConfirmedEvent.Chain, 
                            Hash: request.TransferConfirmedEvent.Hash, 
                            From: request.TransferConfirmedEvent.From,
                            To: request.TransferConfirmedEvent.To, 
                            Value: request.TransferConfirmedEvent.Value, 
                            DateTime: request.TransferConfirmedEvent.DateTime,
                            Erc20Transfers: request.TransferConfirmedEvent.Erc20Transfers?.Select(s => new NotificationCreatedEvent.Erc20Transfer(s.From, s.To, s.Value, s.ContractAddress)).ToList(),
                            Erc721Transfers: request.TransferConfirmedEvent.Erc721Transfers?.Select(s => new NotificationCreatedEvent.Erc721Transfer(s.From, s.To, s.TokenId, s.ContractAddress)).ToList())));
                }
            }
            
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private static List<string>? GetDestinationAddresses(ITransferConfirmedMessage transferConfirmedEvent)
    {
        if(transferConfirmedEvent.Erc20Transfers is null && transferConfirmedEvent.Erc721Transfers is null)
            return [transferConfirmedEvent.To];
        else if(transferConfirmedEvent.Erc721Transfers is not null)
            return [.. transferConfirmedEvent.Erc721Transfers.Select(s => s.To).Distinct()];
        else if(transferConfirmedEvent.Erc20Transfers is not null)
            return [.. transferConfirmedEvent.Erc20Transfers.Select(s => s.To).Distinct()];
        return null;
    }
}
