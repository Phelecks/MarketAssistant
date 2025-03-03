using AutoMapper;
using BaseApplication.Exceptions;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.Resource.Queries.GetResource;

[Authorize(roles = "Administrators")]
public record GetResourceQuery([property: Required] long Id) : IRequest<ResourceDto>;

public class Handler : IRequestHandler<GetResourceQuery, ResourceDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public Handler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ResourceDto> Handle(GetResourceQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.resources.SingleOrDefaultAsync(exp => exp.Id == request.Id, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.Role), request.Id);

        return _mapper.Map<Domain.Entities.Resource, ResourceDto>(entity);
    }
}
