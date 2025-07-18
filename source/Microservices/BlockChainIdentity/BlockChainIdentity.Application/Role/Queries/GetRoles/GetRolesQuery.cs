using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Mappings;
using BaseApplication.Models;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using BlockChainIdentity.Application.Role.Queries.GetRoles;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Role.Client.GetRoles.GetClients;

[Authorize(roles = "Administrators")]
public record GetRolesQuery : PagingInformationRequest, IRequest<PaginatedList<RoleDto>>;

public class Handler : IRequestHandler<GetRolesQuery, PaginatedList<RoleDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public Handler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<RoleDto>> HandleAsync(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Roles
            .Include(inc => inc.WalletRoles).ThenInclude(inc => inc.Role)
            .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
            .ProjectToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy, cancellationToken);
    }
}
