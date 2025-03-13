using BaseApplication.Exceptions;
using Informing.Domain.Entities;
using Informing.Domain.Events.Information;
using MediatR;
using Microsoft.EntityFrameworkCore;
using IApplicationDbContext = Informing.Application.Interfaces.IApplicationDbContext;

namespace Informing.Application.Information.Commands.SendError;


public record SendErrorCommand(string content) : IRequest<Unit>;

public class SendVerificationCodeCommandHandler : IRequestHandler<SendErrorCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public SendVerificationCodeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SendErrorCommand request, CancellationToken cancellationToken)
    {
        var template = await _context.Templates.SingleOrDefaultAsync(exp => exp.InformingType == BaseDomain.Enums.InformingEnums.InformingType.SystemErrorMessage && exp.InformingSendType == BaseDomain.Enums.InformingEnums.InformingSendType.Email, cancellationToken);
        if (template == null) throw new NotFoundException($"Template with informingType: {BaseDomain.Enums.InformingEnums.InformingType.SystemErrorMessage} and informingSendType: {BaseDomain.Enums.InformingEnums.InformingSendType.Email}, not found.");
        var contacts = await _context.Contacts.Where(exp => exp.GroupContacts.Any(gcExp => gcExp.Group.Title.Equals("Administrators"))).ToListAsync(cancellationToken);

        var content = template.Content.Replace("@Content", request.content);


        var entity = new Domain.Entities.Information
        {
            Content = content,
            Title = template.Title,
            Type = InformationType.Email,
            ContactInformations = contacts.Select(s => new ContactInformation
            {
                Contact = s,
            }).ToList()
        };

        await _context.Information.AddAsync(entity, cancellationToken);

        entity.AddDomainEvent(new SystemErrorSentEvent(entity));
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
