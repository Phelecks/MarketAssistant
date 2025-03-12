using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Mappings;
using BaseApplication.Models;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Wallet.Queries.GetWallets;

[Authorize(roles = "Administrators")]
public record GetWalletsQuery : PagingInformationRequest, IRequest<PaginatedList<WalletDto>>;

public class Handler : IRequestHandler<GetWalletsQuery, PaginatedList<WalletDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public Handler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<WalletDto>> Handle(GetWalletsQuery request, CancellationToken cancellationToken)
    {
        return await _context.wallets
            .ProjectTo<WalletDto>(_mapper.ConfigurationProvider)
            .ProjectToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy, cancellationToken);
    }
}
