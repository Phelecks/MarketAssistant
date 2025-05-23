﻿using BaseApplication.Exceptions;
using LogProcessor.Domain.Events.Transfer;
using LogProcessor.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LogProcessor.Application.Transfer.Commands.MarkTransferAsFailed;

public record MarkTransferAsFailed([property: Required] string Hash, [property: Required] Nethereum.Signer.Chain Chain) : IRequest<Unit>;

public class Handler(IApplicationDbContext context) : IRequestHandler<MarkTransferAsFailed, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> Handle(MarkTransferAsFailed request, CancellationToken cancellationToken)
    {
        var entity = await _context.Transfers.SingleOrDefaultAsync(exp => exp.Hash.Equals(request.Hash) && exp.Chain == request.Chain, cancellationToken) 
            ?? throw new NotFoundException(nameof(Transfer), request.Hash);
        entity.State = Domain.Entities.Transfer.TransferState.Failed;
        entity.AddDomainEvent(new TransferMarkedAsFailedEvent(entity, "Transaction reversed or failed on blockchain."));
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
