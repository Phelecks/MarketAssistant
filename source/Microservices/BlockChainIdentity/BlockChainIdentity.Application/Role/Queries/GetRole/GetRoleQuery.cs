using AutoMapper;
using BaseApplication.Exceptions;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using BlockChainIdentity.Application.Role.Queries.GetRole;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Role.Wallet.GetRole.GetWallet;

[Authorize(roles = "Administrators")]
public record GetRoleQuery([property: Required] long Id) : IRequest<RoleDto>;

public class Handler : IRequestHandler<GetRoleQuery, RoleDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public Handler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RoleDto> HandleAsync(GetRoleQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Roles.SingleOrDefaultAsync(exp => exp.Id == request.Id, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.Role), request.Id);

        return _mapper.Map<Domain.Entities.Role, RoleDto>(entity);
    }
}
