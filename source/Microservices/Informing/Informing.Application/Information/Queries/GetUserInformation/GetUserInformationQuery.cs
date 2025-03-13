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
        return await _context.Information
            .Where(exp => exp.Enabled && exp.Type == Domain.Entities.InformationType.News 
                && (exp.ContactInformations.Any(ciExp => ciExp.Contact.UserId.Equals(_identityHelper.GetUserIdentity()))
                || exp.GroupInformations.Any(giExp => giExp.Group.GroupContacts.Any(gcExp => gcExp.Contact.UserId.Equals(_identityHelper.GetUserIdentity())))))
            .ProjectTo<UserInformationDto>(_mapper.ConfigurationProvider)
            .ProjectToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy, cancellationToken);
    }
}
