using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Mappings;
using BaseApplication.Models;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR;

namespace BlockChainIdentity.Application.WalletRole.Queries.GetWalletRoles;

[Authorize(roles = "Administrators")]
public record GetWalletRolesQuery(string WalletAddress) : PagingInformationRequest, IRequest<PaginatedList<WalletRoleDto>>;

public class Handler : IRequestHandler<GetWalletRolesQuery, PaginatedList<WalletRoleDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public Handler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<WalletRoleDto>> Handle(GetWalletRolesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Roles
            .Where(exp => exp.WalletRoles.Any(walletRole => walletRole.walletAddress.Equals(request.WalletAddress)))
            .ProjectTo<WalletRoleDto>(_mapper.ConfigurationProvider)
            .ProjectToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy, cancellationToken);
    }
}
