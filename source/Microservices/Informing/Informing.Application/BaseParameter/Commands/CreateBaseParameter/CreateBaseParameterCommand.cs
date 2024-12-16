using System.ComponentModel.DataAnnotations;
using BaseDomain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using IApplicationDbContext = Informing.Application.Interfaces.IApplicationDbContext;

namespace Informing.Application.BaseParameter.Commands.CreateBaseParameter;


public record CreateBaseParameterCommand(BaseParameterCategory category, BaseParameterField field, string value, long kernelBaseParameterId) : IRequest<long>
{
    /// <summary>
    /// Category
    /// </summary>
    [Required]
    public BaseParameterCategory category { get; } = category;

    /// <summary>
    /// Field
    /// </summary>
    [Required]
    public BaseParameterField field { get; } = field;

    /// <summary>
    /// Value
    /// </summary>
    [Required]
    public string value { get; } = value;

    [Required]
    public long kernelBaseParameterId { get; set; } = kernelBaseParameterId;
}

public class Handler : IRequestHandler<CreateBaseParameterCommand, long>
{
    private readonly IApplicationDbContext _context;

    public Handler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateBaseParameterCommand request, CancellationToken cancellationToken)
    {
        var entity =
            await _context.baseParameters.SingleOrDefaultAsync(
                exp => exp.kernelBaseParameterId == request.kernelBaseParameterId, cancellationToken);
        if (entity is not null)
            entity.value = request.value;
        else
        {
            entity = new Domain.Entities.BaseParameter
            {
                category = request.category,
                field = request.field,
                value = request.value,
                kernelBaseParameterId = request.kernelBaseParameterId
            };

            await _context.baseParameters.AddAsync(entity, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return entity.id;
    }
}
