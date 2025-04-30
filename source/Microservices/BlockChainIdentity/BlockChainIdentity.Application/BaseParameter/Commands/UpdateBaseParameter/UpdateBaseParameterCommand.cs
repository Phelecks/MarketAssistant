using BaseApplication.Exceptions;
using BlockChainIdentity.Domain.Events.BaseParameter;
using MediatR;
using BlockChainIdentity.Application.Interfaces;

namespace BlockChainIdentity.Application.BaseParameter.Commands.UpdateBaseParameter;

//[Authorize(roles = "Administrators")]
public record UpdateBaseParameterCommand : IRequest<Unit>
{
    public UpdateBaseParameterCommand(long id, string value)
    {
        this.id = id;
        this.value = value;
    }

    public long id { get; }

    public string value { get; }
}

public class Handler : IRequestHandler<UpdateBaseParameterCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public Handler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateBaseParameterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.BaseParameters
            .FindAsync([request.id], cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.BaseParameter), request.id);

        entity.Value = request.value;

        entity.AddDomainEvent(new BaseParameterUpdatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
