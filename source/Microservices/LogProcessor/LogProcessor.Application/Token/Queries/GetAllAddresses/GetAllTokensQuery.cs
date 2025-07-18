using LogProcessor.Application.Interfaces;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LogProcessor.Application.Token.Queries.GetAllTokens;

public record GetAllTokensQuery(Nethereum.Signer.Chain Chain) : IRequest<List<Domain.Entities.Token>>;

public class Handler(IApplicationDbContext context) : IRequestHandler<GetAllTokensQuery, List<Domain.Entities.Token>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<List<Domain.Entities.Token>> HandleAsync(GetAllTokensQuery request, CancellationToken cancellationToken)
    {
        return await _context.Tokens.Where(exp => exp.Chain == request.Chain).AsNoTracking().ToListAsync(cancellationToken);
    }
}
