using LogProcessor.Application.Interfaces;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LogProcessor.Application.RpcUrl.Queries.GetAllChains;

public record GetAllChainsQuery() : IRequest<List<Nethereum.Signer.Chain>>;

public class Handler(IApplicationDbContext context) : IRequestHandler<GetAllChainsQuery, List<Nethereum.Signer.Chain>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<List<Nethereum.Signer.Chain>> HandleAsync(GetAllChainsQuery request, CancellationToken cancellationToken)
    {
        return await _context.RpcUrls.Select(s => s.Chain).Distinct().ToListAsync(cancellationToken);
    }
}
