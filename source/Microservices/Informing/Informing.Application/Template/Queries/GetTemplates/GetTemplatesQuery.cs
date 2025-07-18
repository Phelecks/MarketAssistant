using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Mappings;
using BaseApplication.Models;
using BaseApplication.Security;
using Informing.Application.Interfaces;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Informing.Application.Template.Queries.GetTemplates;

[Authorize(roles = "Administrators")]
public record GetTemplatesQuery : PagingInformationRequest, IRequest<PaginatedList<TemplatesDto>>;

public class GetTemplatesQueryHandler : IRequestHandler<GetTemplatesQuery, PaginatedList<TemplatesDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTemplatesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TemplatesDto>> HandleAsync(GetTemplatesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Templates
            .ProjectTo<TemplatesDto>(_mapper.ConfigurationProvider)
            .ProjectToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy, cancellationToken);
    }
}
