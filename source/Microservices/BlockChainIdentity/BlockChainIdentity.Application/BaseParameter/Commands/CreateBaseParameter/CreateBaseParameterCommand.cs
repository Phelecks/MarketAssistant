using System.ComponentModel.DataAnnotations;
using BaseDomain.Enums;
using BlockChainIdentity.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.BaseParameter.Commands.CreateBaseParameter;


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
            await _context.BaseParameters.SingleOrDefaultAsync(
                exp => exp.KernelBaseParameterId == request.kernelBaseParameterId, cancellationToken);
        if (entity is not null)
            entity.Value = request.value;
        else
        {
            entity = new Domain.Entities.BaseParameter
            {
                Category = request.category,
                Field = request.field,
                Value = request.value,
                KernelBaseParameterId = request.kernelBaseParameterId
            };

            await _context.BaseParameters.AddAsync(entity, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
