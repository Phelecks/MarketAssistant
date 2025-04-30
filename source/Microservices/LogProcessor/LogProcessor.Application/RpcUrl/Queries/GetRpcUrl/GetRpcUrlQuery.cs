using System.ComponentModel.DataAnnotations;
using BaseApplication.Exceptions;
using BaseApplication.Interfaces;
using LogProcessor.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogProcessor.Application.RpcUrl.Queries.GetRpcUrl;

public record GetRpcUrlQuery([property: Required] Nethereum.Signer.Chain Chain) : IRequest<Domain.Entities.RpcUrl>;

public class Handler(IApplicationDbContext context, IShuffleService<Domain.Entities.RpcUrl> shuffleService) : IRequestHandler<GetRpcUrlQuery, Domain.Entities.RpcUrl>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IShuffleService<Domain.Entities.RpcUrl> _shuffleService = shuffleService;

    public async Task<Domain.Entities.RpcUrl> Handle(GetRpcUrlQuery request, CancellationToken cancellationToken)
    {
        var rpcUrls = await _context.RpcUrls.Where(exp => exp.Chain == request.Chain).ToListAsync(cancellationToken);
        if(rpcUrls.Count == 0) throw new NotFoundException(nameof(Domain.Entities.RpcUrl), request.Chain);
        var result = _shuffleService.Shuffle(rpcUrls);
        return result;
    }
}
