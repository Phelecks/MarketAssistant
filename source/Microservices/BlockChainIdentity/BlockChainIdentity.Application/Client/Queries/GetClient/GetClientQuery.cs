using AutoMapper;
using BaseApplication.Exceptions;
using BlockChainIdentity.Application.Interfaces;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.Client.Queries.GetClient;

//[Authorize(roles = "Administrators")]
public record GetClientQuery([property: Required] long Id) : IRequest<ClientDto>;

public class Handler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetClientQuery, ClientDto>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<ClientDto> HandleAsync(GetClientQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Clients.Include(inc => inc.ClientResources).ThenInclude(inc => inc.Resource).SingleOrDefaultAsync(exp => exp.Id == request.Id, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.Client), request.Id);

        return _mapper.Map<Domain.Entities.Client, ClientDto>(entity);
    }
}
