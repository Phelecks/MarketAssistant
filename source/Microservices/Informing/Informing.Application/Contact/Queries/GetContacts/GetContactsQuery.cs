using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Mappings;
using BaseApplication.Models;
using BaseApplication.Security;
using Informing.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Informing.Application.Contact.Queries.GetContacts;

[Authorize(roles = "Administrators")]
public record GetContactsQuery : PagingInformationRequest, IRequest<PaginatedList<ContactsDto>>;

public class GetContactsQueryHandler : IRequestHandler<GetContactsQuery, PaginatedList<ContactsDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetContactsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ContactsDto>> Handle(GetContactsQuery request, CancellationToken cancellationToken)
    {
        return await _context.contacts
            .ProjectTo<ContactsDto>(_mapper.ConfigurationProvider)
            .ProjectToPaginatedListAsync(request.pageNumber, request.pageSize, request.orderBy, cancellationToken);
    }
}
