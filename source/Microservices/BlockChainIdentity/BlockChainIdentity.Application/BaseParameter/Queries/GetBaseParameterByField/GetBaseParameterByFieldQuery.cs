using AutoMapper;
using BaseApplication.Exceptions;
using BlockChainIdentity.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.BaseParameter.Queries.GetBaseParameterByField;

//[Authorize(roles = "Administrators")]
public record GetBaseParameterByFieldQuery([property: Required] BaseDomain.Enums.BaseParameterField field) : IRequest<BaseParameterDto>;

public class Handler : IRequestHandler<GetBaseParameterByFieldQuery, BaseParameterDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public Handler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseParameterDto> Handle(GetBaseParameterByFieldQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.baseParameters.SingleOrDefaultAsync(exp => exp.field == request.field, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.BaseParameter), request.field);

        return _mapper.Map<Domain.Entities.BaseParameter, BaseParameterDto>(entity);
    }
}
