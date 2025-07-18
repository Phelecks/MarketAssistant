using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Mappings;
using BaseApplication.Models;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR.Interfaces;

namespace BlockChainIdentity.Application.Client.Queries.GetClients;

[Authorize(roles = "Administrators")]
public record GetClientsQuery : PagingInformationRequest, IRequest<PaginatedList<ClientDto>>;

public class Handler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetClientsQuery, PaginatedList<ClientDto>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<PaginatedList<ClientDto>> HandleAsync(GetClientsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Clients
            //.Include(inc => inc.clientResources).ThenInclude(inc => inc.resource)
            .ProjectTo<ClientDto>(_mapper.ConfigurationProvider)
            .ProjectToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy, cancellationToken);
    }
}
