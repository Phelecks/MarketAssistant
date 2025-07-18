using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Mappings;
using BaseApplication.Models;
using BaseApplication.Security;
using Informing.Application.Interfaces;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Informing.Application.Information.Queries.GetInformation;

[Authorize(roles = "Administrators")]
public record GetInformationQuery : PagingInformationRequest, IRequest<PaginatedList<InformationDto>>;

public class GetInformationQueryHandler : IRequestHandler<GetInformationQuery, PaginatedList<InformationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInformationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<InformationDto>> HandleAsync(GetInformationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Information
            .ProjectTo<InformationDto>(_mapper.ConfigurationProvider)
            .ProjectToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy, cancellationToken);
    }
}
