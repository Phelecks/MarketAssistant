using System.ComponentModel.DataAnnotations;
using BlockProcessor.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlockProcessor.Application.RpcUrl.Queries.GetWaitIntervalOfBlockProgress;

public record GetWaitIntervalOfBlockProgressQuery([property: Required] Nethereum.Signer.Chain Chain) : IRequest<int>;

public class Handler(IApplicationDbContext context) : IRequestHandler<GetWaitIntervalOfBlockProgressQuery, int>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<int> Handle(GetWaitIntervalOfBlockProgressQuery request, CancellationToken cancellationToken)
    {
        var rpcUrl = await _context.RpcUrls.FirstOrDefaultAsync(exp => exp.Chain == request.Chain, cancellationToken);
        if(rpcUrl is null) return 10;
        return rpcUrl.WaitInterval;
    }
}
