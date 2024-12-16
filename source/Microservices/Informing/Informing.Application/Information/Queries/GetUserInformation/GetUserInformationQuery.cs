using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Mappings;
using BaseApplication.Models;
using BaseApplication.Security;
using IdentityHelper.Helpers;
using Informing.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Informing.Application.Information.Queries.GetUserInformation;

[Authorize]
public record GetUserInformationQuery : PagingInformationRequest, IRequest<PaginatedList<UserInformationDto>>;

public class GetUserInformationQueryHandler : IRequestHandler<GetUserInformationQuery, PaginatedList<UserInformationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IIdentityHelper _identityHelper;

    public GetUserInformationQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityHelper identityHelper)
    {
        _context = context;
        _mapper = mapper;
        _identityHelper = identityHelper;
    }

    public async Task<PaginatedList<UserInformationDto>> Handle(GetUserInformationQuery request, CancellationToken cancellationToken)
    {
        return await _context.information
            .Where(exp => exp.enabled && exp.type == Domain.Entities.InformationType.News 
                && (exp.contactInformations.Any(ciExp => ciExp.contact.userId.Equals(_identityHelper.GetUserIdentity()))
                || exp.groupInformations.Any(giExp => giExp.group.groupContacts.Any(gcExp => gcExp.contact.userId.Equals(_identityHelper.GetUserIdentity())))))
            .ProjectTo<UserInformationDto>(_mapper.ConfigurationProvider)
            .ProjectToPaginatedListAsync(request.pageNumber, request.pageSize, request.orderBy, cancellationToken);
    }
}
