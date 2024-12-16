using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Mappings;
using BaseApplication.Models;
using Informing.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Informing.Application.GroupContacts.Queries.GetGroups;

public record GetGroupContactsQuery : PagingInformationRequest, IRequest<PaginatedList<GroupContactDto>>;

public class GetGroupsQueryHandler : IRequestHandler<GetGroupContactsQuery, PaginatedList<GroupContactDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetGroupsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GroupContactDto>> Handle(GetGroupContactsQuery request, CancellationToken cancellationToken)
    {
        return await _context.groupContacts
            .ProjectTo<GroupContactDto>(_mapper.ConfigurationProvider)
            .ProjectToPaginatedListAsync(request.pageNumber, request.pageSize, request.orderBy, cancellationToken);
    }
}
