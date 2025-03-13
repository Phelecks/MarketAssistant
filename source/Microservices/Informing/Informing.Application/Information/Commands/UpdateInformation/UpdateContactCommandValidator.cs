using FluentValidation;
using Informing.Application.Interfaces;

namespace Informing.Application.Information.Commands.UpdateInformation;

public class UpdateContactCommandValidator : AbstractValidator<UpdateInformationCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateContactCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.id)
            .MustAsync(BeNewsAsync).WithMessage("Only news can be updated.");

    }

    private async Task<bool> BeNewsAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await _context.Information
            .FindAsync(new object[] { id }, cancellationToken);
        if (entity == null) return false;

        if (entity.Type == Domain.Entities.InformationType.News)
            return true;

        return false;
    }
}
