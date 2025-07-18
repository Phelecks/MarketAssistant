using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Mappings;
using BaseApplication.Models;
using Informing.Application.Interfaces;
using MediatR.Interfaces;

namespace Informing.Application.GroupContacts.Queries.GetGroupContacts;

public record GetGroupContactsQuery : PagingInformationRequest, IRequest<PaginatedList<GroupContactDto>>;

public class GetGroupsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetGroupContactsQuery, PaginatedList<GroupContactDto>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<PaginatedList<GroupContactDto>> HandleAsync(GetGroupContactsQuery request, CancellationToken cancellationToken)
    {
        return await _context.GroupContacts
            .ProjectTo<GroupContactDto>(_mapper.ConfigurationProvider)
            .ProjectToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy, cancellationToken);
    }
}
