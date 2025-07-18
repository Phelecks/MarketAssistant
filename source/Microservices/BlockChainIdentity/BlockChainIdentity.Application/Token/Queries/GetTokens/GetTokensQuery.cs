using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Mappings;
using BaseApplication.Models;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR.Interfaces;

namespace BlockChainIdentity.Application.Token.Queries.GetTokens;

[Authorize(roles = "Administrators")]
public record GetTokensQuery : PagingInformationRequest, IRequest<PaginatedList<TokenDto>>;

public class Handler : IRequestHandler<GetTokensQuery, PaginatedList<TokenDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public Handler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TokenDto>> HandleAsync(GetTokensQuery request, CancellationToken cancellationToken)
    {
        return await _context.Tokens
            .ProjectTo<TokenDto>(_mapper.ConfigurationProvider)
            .ProjectToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy, cancellationToken);
    }
}
