using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Mappings;
using BaseApplication.Models;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using BlockChainIdentity.Application.Role.Queries.GetRoles;
using MediatR;
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

    public async Task<PaginatedList<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await _context.roles
            .Include(inc => inc.walletRoles).ThenInclude(inc => inc.role)
            .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
            .ProjectToPaginatedListAsync(request.pageNumber, request.pageSize, request.orderBy, cancellationToken);
    }
}
