﻿using BaseDomain.Enums;
using FluentValidation;
using Informing.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Informing.Application.BaseParameter.Commands.CreateBaseParameter;

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
        return await _context.BaseParameters
            .AllAsync(l => l.Field != field, cancellationToken);
    }
}
