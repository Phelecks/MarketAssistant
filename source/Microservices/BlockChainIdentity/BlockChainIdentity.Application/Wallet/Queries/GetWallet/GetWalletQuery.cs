using AutoMapper;
using BaseApplication.Exceptions;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.Wallet.Queries.GetWallet;

[Authorize(roles = "Administrators")]
public record GetWalletQuery([property: Required] string Address) : IRequest<WalletDto>;

public class Handler : IRequestHandler<GetWalletQuery, WalletDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public Handler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WalletDto> Handle(GetWalletQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Wallets.SingleOrDefaultAsync(exp => exp.address.Equals(request.Address), cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.Wallet), request.Address);

        return _mapper.Map<Domain.Entities.Wallet, WalletDto>(entity);
    }
}
