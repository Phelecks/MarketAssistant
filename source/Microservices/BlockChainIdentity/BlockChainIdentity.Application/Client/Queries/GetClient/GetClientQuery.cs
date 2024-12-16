﻿using AutoMapper;
using BaseApplication.Exceptions;
using BlockChainIdentity.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.Client.Queries.GetClient;

//[Authorize(roles = "Administrators")]
public record GetClientQuery([property: Required] long id) : IRequest<ClientDto>;

public class Handler : IRequestHandler<GetClientQuery, ClientDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public Handler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ClientDto> Handle(GetClientQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.clients.Include(inc => inc.clientResources).ThenInclude(inc => inc.resource).SingleOrDefaultAsync(exp => exp.id == request.id, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.BaseParameter), request.id);

        return _mapper.Map<Domain.Entities.Client, ClientDto>(entity);
    }
}