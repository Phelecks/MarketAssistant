using BaseDomain.Helpers;
using BlockProcessor.Application.Interfaces;
using BlockProcessor.Domain.Events.Transfer;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace BlockProcessor.Application.Transfer.Commands.InitiateTransfer;

public record InitiateTransferCommand([property: Required] string Hash, [property: Required] string From,
    [property: Required] string To, [property: Required] BigInteger Value, [property: Required] BigInteger GasUsed,
    [property: Required] BigInteger EffectiveGasPrice, [property: Required] BigInteger CumulativeGasUsed,
    [property: Required] BigInteger BlockNumber, [property: Required] DateTime ConfimedDatetime,
    [property: Required] Nethereum.Signer.Chain Chain,
    List<Erc20Transfer>? Erc20Transfers, List<Erc721Transfer>? Erc721Transfers) : IRequest<Unit>;

 public record Erc20Transfer([property: Required] string From, [property: Required] string To, [property: Required] BigInteger Value, [property: Required] string ContractAddress);
 public record Erc721Transfer([property: Required] string From, [property: Required] string To, [property: Required] string ContractAddress, [property: Required] BigInteger TokenId);

public class Handler(IApplicationDbContext context) : IRequestHandler<InitiateTransferCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> Handle(InitiateTransferCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Transfer
        {
            Id = Guid.NewGuid(),
            Hash = request.Hash,
            From = request.From,
            To = request.To,
            Value = request.Value.ConvertToDecimal(18),
            GasUsed = request.GasUsed.ConvertToDecimal(0),
            EffectiveGasPrice = request.EffectiveGasPrice.ConvertToDecimal(18),
            CumulativeGasUsed = request.CumulativeGasUsed.ConvertToDecimal(18),
            BlockNumber = (long)request.BlockNumber,
            ConfirmedDatetime = request.ConfimedDatetime,
            Chain = request.Chain,
            State = Domain.Entities.Transfer.TransferState.Initiated,
            Erc20Transfers = request.Erc20Transfers?.Select(erc20Transfer => new Domain.Entities.Erc20Transfer
            {
                From = erc20Transfer.From,
                To = erc20Transfer.To,
                ContractAddress = erc20Transfer.ContractAddress,
                Value = erc20Transfer.Value.ConvertToDecimal(6)
            }).ToList(),
            Erc721Transfers = request.Erc721Transfers?.Select(erc721Transfer => new Domain.Entities.Erc721Transfer
            {
                From = erc721Transfer.From,
                To = erc721Transfer.To,
                TokenId = (long)erc721Transfer.TokenId,
                ContractAddress = erc721Transfer.ContractAddress
            }).ToList()
        };

        await _context.Transfers.AddAsync(entity, cancellationToken);
        entity.AddDomainEvent(new TransferInitiatedEvent(entity));
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
