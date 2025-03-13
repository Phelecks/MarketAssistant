using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Mappings;
using BaseApplication.Models;
using BaseApplication.Security;
using Informing.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Informing.Application.Group.Queries.GetGroups;

[Authorize(roles = "Administrators")]
public record GetGroupsQuery : PagingInformationRequest, IRequest<PaginatedList<GroupDto>>;

public class GetGroupsQueryHandler : IRequestHandler<GetGroupsQuery, PaginatedList<GroupDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetGroupsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GroupDto>> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Groups
            .ProjectTo<GroupDto>(_mapper.ConfigurationProvider)
            .ProjectToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy, cancellationToken);
    }
}
