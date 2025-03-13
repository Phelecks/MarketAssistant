using AutoMapper;
using BaseApplication.Exceptions;
using BlockChainIdentity.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.BaseParameter.Queries.GetBaseParameterByField;

//[Authorize(roles = "Administrators")]
public record GetBaseParameterByFieldQuery([property: Required] BaseDomain.Enums.BaseParameterField Field) : IRequest<BaseParameterDto>;

public class Handler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetBaseParameterByFieldQuery, BaseParameterDto>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<BaseParameterDto> Handle(GetBaseParameterByFieldQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.BaseParameters.SingleOrDefaultAsync(exp => exp.Field == request.Field, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.BaseParameter), request.Field);

        return _mapper.Map<Domain.Entities.BaseParameter, BaseParameterDto>(entity);
    }
}
