using AutoMapper;
using BaseApplication.Exceptions;
using BlockChainIdentity.Application.Interfaces;
using MediatR;

namespace BlockChainIdentity.Application.BaseParameter.Queries.GetBaseParameter;

//[Authorize(roles = "Administrators")]
public record GetBaseParameterQuery(long Id) : IRequest<BaseParameterDto>;

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
            .FindAsync([request.Id], cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.BaseParameter), request.Id);

        return _mapper.Map<Domain.Entities.BaseParameter, BaseParameterDto>(entity);
    }
}
