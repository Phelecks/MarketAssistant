using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Mappings;
using BaseApplication.Models;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.Token.Queries.GetWalletTokens;

[Authorize(roles = "Administrators")]
public record GetWalletTokensQuery([property: Required] string address) : PagingInformationRequest, IRequest<PaginatedList<WalletTokenDto>>;

public class Handler : IRequestHandler<GetWalletTokensQuery, PaginatedList<WalletTokenDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public Handler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<WalletTokenDto>> Handle(GetWalletTokensQuery request, CancellationToken cancellationToken)
    {
        return await _context.tokens
            .Where(exp => exp.walletAddress.Equals(request.address))
            .ProjectTo<WalletTokenDto>(_mapper.ConfigurationProvider)
            .ProjectToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy, cancellationToken);
    }
}
