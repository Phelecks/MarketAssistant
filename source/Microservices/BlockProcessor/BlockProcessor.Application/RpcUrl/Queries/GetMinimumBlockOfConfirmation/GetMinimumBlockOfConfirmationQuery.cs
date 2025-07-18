using System.ComponentModel.DataAnnotations;
using BlockProcessor.Application.Interfaces;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlockProcessor.Application.RpcUrl.Queries.GetMinimumBlockOfConfirmation;

public record GetMinimumBlockOfConfirmationQuery([property: Required] Nethereum.Signer.Chain Chain) : IRequest<int>;

public class Handler(IApplicationDbContext context) : IRequestHandler<GetMinimumBlockOfConfirmationQuery, int>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<int> HandleAsync(GetMinimumBlockOfConfirmationQuery request, CancellationToken cancellationToken)
    {
        var rpcUrl = await _context.RpcUrls.FirstOrDefaultAsync(exp => exp.Chain == request.Chain, cancellationToken);
        if(rpcUrl is null) return 3;
        return rpcUrl.BlockOfConfirmation;
    }
}
