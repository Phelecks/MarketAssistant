using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Mappings;
using BaseApplication.Models;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Resource.Queries.GetResources;

[Authorize(roles = "Administrators")]
public record GetResourcesQuery : PagingInformationRequest, IRequest<PaginatedList<ResourceDto>>;

public class Handler : IRequestHandler<GetResourcesQuery, PaginatedList<ResourceDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public Handler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ResourceDto>> HandleAsync(GetResourcesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Resources
            .ProjectTo<ResourceDto>(_mapper.ConfigurationProvider)
            .ProjectToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy, cancellationToken);
    }
}
