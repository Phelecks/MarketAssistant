using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Interfaces;
using BaseApplication.Mappings;
using BaseApplication.Models;
using BaseApplication.Security;
using BlockProcessor.Application.Interfaces;
using MediatR;

namespace BlockProcessor.Application.RpcUrl.Queries.GetRpcUrls;

[Authorize(roles = "Administrators")]
public record GetRpcUrlsQuery(bool bypassCache, string cacheKey, int? expireInMinutes = 24 * 60) : PagingInformationRequest, IRequest<PaginatedList<RpcUrlDto>>, ICacheableMediatrQuery;

public class Handler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetRpcUrlsQuery, PaginatedList<RpcUrlDto>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<PaginatedList<RpcUrlDto>> Handle(GetRpcUrlsQuery request, CancellationToken cancellationToken)
    {
        return await _context.RpcUrls
            .ProjectTo<RpcUrlDto>(_mapper.ConfigurationProvider)
            .ProjectToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy, cancellationToken);
    }
}
