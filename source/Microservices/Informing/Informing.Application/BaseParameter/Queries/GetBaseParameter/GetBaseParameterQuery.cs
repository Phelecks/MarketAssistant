﻿using AutoMapper;
using BaseApplication.Exceptions;
using BaseApplication.Security;
using Informing.Application.Interfaces;
using MediatR;

namespace Informing.Application.BaseParameter.Queries.GetBaseParameter;

//[Authorize(roles = "Administrators")]
public record GetBaseParameterQuery(long id) : IRequest<BaseParameterDto>;

public class Handler : IRequestHandler<GetBaseParameterQuery, BaseParameterDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public Handler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseParameterDto> Handle(GetBaseParameterQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.baseParameters
            .FindAsync(new object[] { request.id }, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.BaseParameter), request.id);

        return _mapper.Map<Domain.Entities.BaseParameter, BaseParameterDto>(entity);
    }
}
