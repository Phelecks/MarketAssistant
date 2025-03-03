using System.ComponentModel.DataAnnotations;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Client.Commands.CreateClient;

[Authorize(roles = "Administrators")]
public record CreateClientCommand([property: Required] string ClientId, [property: Required] string ClientSecret, [property: Required] Uri Uri, [property: Required] bool Enabled, [property: Required] string Statement, [property: Required] string Version, [property: Required] List<long> Resources, int? tokenLifeTimeInSeconds) : IRequest<long>;

public class Handler : IRequestHandler<CreateClientCommand, long>
{
    private readonly IApplicationDbContext _context;

    public Handler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var resources = await _context.resources.Where(exp => request.Resources.Any(resourceId => resourceId == exp.Id)).ToListAsync(cancellationToken);

        int tokenLifeTime;
        if (request.tokenLifeTimeInSeconds.HasValue) tokenLifeTime = request.tokenLifeTimeInSeconds.Value;
        else
        {
            var baseParameter = await _context.baseParameters.SingleOrDefaultAsync(exp => exp.field == BaseDomain.Enums.BaseParameterField.BlockChainIdentityDefaultGeneratedSiweMessageLifeTime, cancellationToken);
            if (baseParameter == null) tokenLifeTime = 60;
            else tokenLifeTime = int.Parse(baseParameter.value);
        }

        var entity = new Domain.Entities.Client
        {
            clientId = request.ClientId,
            clientSecret = request.ClientSecret,
            tokenLifeTimeInSeconds = tokenLifeTime,
            enabled = request.Enabled,
            uri = request.Uri,
            statement = request.Statement,
            version = request.Version,
            clientResources = resources.Select(s => new Domain.Entities.ClientResource
            {
                resourceId = s.Id
            }).ToList()
        };

        await _context.clients.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
