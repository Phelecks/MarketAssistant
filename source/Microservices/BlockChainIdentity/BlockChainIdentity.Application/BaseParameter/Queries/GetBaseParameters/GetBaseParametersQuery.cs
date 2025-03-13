using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Mappings;
using BaseApplication.Models;
using BlockChainIdentity.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.BaseParameter.Queries.GetBaseParameters;

//[Authorize(roles = "Administrators")]
public record GetBaseParametersQuery : PagingInformationRequest, IRequest<PaginatedList<BaseParametersDto>>;

public class Handler : IRequestHandler<GetBaseParametersQuery, PaginatedList<BaseParametersDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public Handler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<BaseParametersDto>> Handle(GetBaseParametersQuery request, CancellationToken cancellationToken)
    {
        return await _context.BaseParameters
            .ProjectTo<BaseParametersDto>(_mapper.ConfigurationProvider)
            .OrderBy(t => t.field)
            .ProjectToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy, cancellationToken);
    }
}
