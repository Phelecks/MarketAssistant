﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseApplication.Mappings;
using BaseApplication.Models;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.Client.Queries.GetClientResources;

[Authorize(roles = "Administrators")]
public record GetClientResourcesQuery([property: Required]long clientId) : PagingInformationRequest, IRequest<PaginatedList<ResourceDto>>;

public class Handler : IRequestHandler<GetClientResourcesQuery, PaginatedList<ResourceDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public Handler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ResourceDto>> Handle(GetClientResourcesQuery request, CancellationToken cancellationToken)
    {
        return await _context.resources
            .Where(exp => exp.clientResources.Any(clientResource => clientResource.clientId == request.clientId))
            .ProjectTo<ResourceDto>(_mapper.ConfigurationProvider)
            .ProjectToPaginatedListAsync(request.pageNumber, request.pageSize, request.orderBy, cancellationToken);
    }
}
