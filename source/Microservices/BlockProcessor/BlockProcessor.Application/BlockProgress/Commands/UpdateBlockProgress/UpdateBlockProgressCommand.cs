using System.ComponentModel.DataAnnotations;
using System.Numerics;
using BlockProcessor.Application.Interfaces;
using BlockProcessor.Domain.Events.BlockProgress;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlockProcessor.Application.BlockProgress.Commands.UpdateBlockProgress;


public record UpdateBlockProgressCommand([property: Required] Nethereum.Signer.Chain Chain, [property: Required] BigInteger BlockNumber) : IRequest<Unit>;

public class Handler(IApplicationDbContext context) : IRequestHandler<UpdateBlockProgressCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> Handle(UpdateBlockProgressCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.BlockProgresses.SingleOrDefaultAsync(exp => exp.Chain == request.Chain, cancellationToken);
        if (entity is null)
        {
            entity = new Domain.Entities.BlockProgress
            {
                BlockNumber = (long)request.BlockNumber,
                Chain = request.Chain
            };
            await _context.BlockProgresses.AddAsync(entity, cancellationToken);
        }
        else
        {
            if(entity.BlockNumber == (long)request.BlockNumber) 
                return Unit.Value;
            entity.BlockNumber = (long)request.BlockNumber;
        }

        entity.AddDomainEvent(new BlockProgressedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}