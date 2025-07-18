using System.ComponentModel.DataAnnotations;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Client.Commands.CreateClient;

[Authorize(roles = "Administrators")]
public record CreateClientCommand([property: Required] string ClientId, [property: Required] string ClientSecret,
    [property: Required] Uri Uri, [property: Required] bool Enabled, [property: Required] string Statement,
    [property: Required] string Version, [property: Required] List<long> Resources, int? tokenLifeTimeInSeconds) 
    : IRequest<long>;

public class Handler(IApplicationDbContext context) : IRequestHandler<CreateClientCommand, long>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<long> HandleAsync(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var resources = await _context.Resources.Where(exp => request.Resources.Any(resourceId => resourceId == exp.Id)).ToListAsync(cancellationToken);

        int tokenLifeTime;
        if (request.tokenLifeTimeInSeconds.HasValue) 
            tokenLifeTime = request.tokenLifeTimeInSeconds.Value;
        else
            tokenLifeTime = 60 * 60; // 1 hour

        var entity = new Domain.Entities.Client
        {
            ClientId = request.ClientId,
            ClientSecret = request.ClientSecret,
            TokenLifeTimeInSeconds = tokenLifeTime,
            Enabled = request.Enabled,
            Uri = request.Uri,
            Statement = request.Statement,
            Version = request.Version,
            ClientResources = [.. resources.Select(s => new Domain.Entities.ClientResource
            {
                ResourceId = s.Id
            })]
        };

        await _context.Clients.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
