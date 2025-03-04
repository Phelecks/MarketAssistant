using System.ComponentModel.DataAnnotations;
using BaseApplication.Exceptions;
using BlockProcessor.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlockProcessor.Application.RpcUrl.Queries.GetRpcUrl;

public record GetRpcUrlQuery([property: Required] Nethereum.Signer.Chain Chain) : IRequest<string>;

public class Handler(IApplicationDbContext context) : IRequestHandler<GetRpcUrlQuery, string>
{
    private readonly IApplicationDbContext _context = context;
    private readonly Random _random = new();

    public async Task<string> Handle(GetRpcUrlQuery request, CancellationToken cancellationToken)
    {
        var rpcUrls = await _context.RpcUrls.Where(exp => exp.Chain == request.Chain).Select(s => s.Uri).ToListAsync(cancellationToken);
        if(rpcUrls.Count == 0) throw new NotFoundException(nameof(Domain.Entities.RpcUrl), request.Chain);
        return GetRandomUrl(rpcUrls);
    }

    private string GetRandomUrl(List<Uri> addresses)
    {
        var span = new Span<Uri>([.. addresses]);
        _random.Shuffle<Uri>(span);
        return span.ToArray()[0].ToString();
    }
}
