using BaseDomain.Enums;
using BlockChainIdentity.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.BaseParameter.Commands.CreateBaseParameter;

public class CreateBaseParameterCommandValidator : AbstractValidator<CreateBaseParameterCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateBaseParameterCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.field)
            .MustAsync(BeUniqueFieldAsync).WithMessage("The specified field already exists.");
    }

    public async Task<bool> BeUniqueFieldAsync(BaseParameterField field, CancellationToken cancellationToken)
    {
        return await _context.baseParameters
            .AllAsync(l => l.field != field, cancellationToken);
    }
}
