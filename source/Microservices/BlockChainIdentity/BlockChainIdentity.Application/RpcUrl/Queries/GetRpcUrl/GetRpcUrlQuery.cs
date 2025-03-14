using System.ComponentModel.DataAnnotations;
using BaseApplication.Exceptions;
using BaseApplication.Interfaces;
using BlockChainIdentity.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.RpcUrl.Queries.GetRpcUrl;

public record GetRpcUrlQuery([property: Required] Nethereum.Signer.Chain Chain) : IRequest<string>;

public class Handler(IApplicationDbContext context, IShuffleService<Uri> shuffleService) : IRequestHandler<GetRpcUrlQuery, string>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IShuffleService<Uri> _shuffleService = shuffleService;

    public async Task<string> Handle(GetRpcUrlQuery request, CancellationToken cancellationToken)
    {
        var rpcUrls = await _context.RpcUrls.Where(exp => exp.Chain == request.Chain).Select(s => s.Uri).ToListAsync(cancellationToken);
        if(rpcUrls.Count == 0) throw new NotFoundException(nameof(Domain.Entities.RpcUrl), request.Chain);
        var result = _shuffleService.Shuffle(rpcUrls);
        return result.ToString();
    }
}
