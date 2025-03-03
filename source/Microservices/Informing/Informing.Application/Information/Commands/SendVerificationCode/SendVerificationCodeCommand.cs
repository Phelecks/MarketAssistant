using BaseApplication.Exceptions;
using Informing.Domain.Entities;
using Informing.Domain.Events.Information;
using MediatR;
using Microsoft.EntityFrameworkCore;
using IApplicationDbContext = Informing.Application.Interfaces.IApplicationDbContext;

namespace Informing.Application.Information.Commands.SendVerificationCode;


public record SendVerificationCodeCommand(string userId, BaseDomain.Enums.InformingEnums.InformingSendType sendType, string verificationCode) : IRequest<long>;

public class SendVerificationCodeCommandHandler : IRequestHandler<SendVerificationCodeCommand, long>
{
    private readonly IApplicationDbContext _context;

    public SendVerificationCodeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(SendVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        var template = await _context.templates.SingleOrDefaultAsync(exp => exp.informingType == BaseDomain.Enums.InformingEnums.InformingType.VerifyEmailAddress && exp.informingSendType == BaseDomain.Enums.InformingEnums.InformingSendType.Email, cancellationToken);
        if (template == null) throw new NotFoundException($"Template with informingType: {BaseDomain.Enums.InformingEnums.InformingType.VerifyEmailAddress} and informingSendType: {BaseDomain.Enums.InformingEnums.InformingSendType.Email}, not found.");
        var contact = await _context.contacts.SingleAsync(exp => exp.userId.Equals(request.userId), cancellationToken);
        
        var content = template.content.Replace("@VerificationCode", request.verificationCode);

        var entity = new Domain.Entities.Information
        {
            content = content,
            title = template.title,
            type = InformationType.Email,
            contactInformations = new List<ContactInformation>
            { 
                new ContactInformation
                {
                    contact = contact
                }
            }
        };

        if (request.sendType == BaseDomain.Enums.InformingEnums.InformingSendType.Email)
            entity.AddDomainEvent(new VerificationCodeByEmailSentEvent(entity));

        await _context.information.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
