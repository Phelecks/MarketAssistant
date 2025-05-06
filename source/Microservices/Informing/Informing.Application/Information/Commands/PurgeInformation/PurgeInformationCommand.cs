using BaseApplication.Security;
using Informing.Application.Interfaces;
using MediatR;

namespace Informing.Application.Information.Commands.PurgeInformation;

[Authorize(roles = "Administrators")]
public record PurgeInformationCommand : IRequest<Unit>;

public class PurgeInformationCommandHandler(IApplicationDbContext context) : IRequestHandler<PurgeInformationCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> Handle(PurgeInformationCommand request, CancellationToken cancellationToken)
    {
        _context.Information.RemoveRange(_context.Information);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
