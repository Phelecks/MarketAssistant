using BaseApplication.Exceptions;
using BaseApplication.Security;
using Informing.Application.Interfaces;
using Informing.Domain.Events.BaseParameter;
using MediatR;

namespace Informing.Application.BaseParameter.Commands.UpdateBaseParameter;

//[Authorize(roles = "Administrators")]
public record UpdateBaseParameterCommand : IRequest
{
    public UpdateBaseParameterCommand(long id, string value)
    {
        this.id = id;
        this.value = value;
    }

    public long id { get; }

    public string value { get; }
}

public class Handler : IRequestHandler<UpdateBaseParameterCommand>
{
    private readonly IApplicationDbContext _context;

    public Handler(IApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task<Unit> Handle(UpdateBaseParameterCommand request, CancellationToken cancellationToken)
    //{
    //    var entity = await _context.baseParameters
    //        .FindAsync(new object[] { request.id }, cancellationToken);

    //    if (entity == null)
    //        throw new NotFoundException(nameof(Domain.Entities.BaseParameter), request.id);

    //    entity.value = request.value;

    //    entity.AddDomainEvent(new BaseParameterUpdatedEvent(entity));

    //    await _context.SaveChangesAsync(cancellationToken);

    //    return Unit.Value;
    //}
    public async Task Handle(UpdateBaseParameterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.baseParameters
            .FindAsync(new object[] { request.id }, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.BaseParameter), request.id);

        entity.value = request.value;

        entity.AddDomainEvent(new BaseParameterUpdatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return;
    }
}
