using BaseApplication.Exceptions;
using Informing.Domain.Entities;
using Informing.Domain.Events.Information;
using MediatR.Interfaces;
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

    public async Task<long> HandleAsync(SendVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        var template = await _context.Templates.SingleOrDefaultAsync(exp => exp.InformingType == BaseDomain.Enums.InformingEnums.InformingType.VerifyEmailAddress && exp.InformingSendType == BaseDomain.Enums.InformingEnums.InformingSendType.Email, cancellationToken);
        if (template == null) throw new NotFoundException($"Template with informingType: {BaseDomain.Enums.InformingEnums.InformingType.VerifyEmailAddress} and informingSendType: {BaseDomain.Enums.InformingEnums.InformingSendType.Email}, not found.");
        var contact = await _context.Contacts.SingleAsync(exp => exp.UserId.Equals(request.userId), cancellationToken);
        
        var content = template.Content.Replace("@VerificationCode", request.verificationCode);

        var entity = new Domain.Entities.Information
        {
            Content = content,
            Title = template.Title,
            Type = InformationType.Email,
            ContactInformations = new List<ContactInformation>
            { 
                new ContactInformation
                {
                    Contact = contact
                }
            }
        };

        if (request.sendType == BaseDomain.Enums.InformingEnums.InformingSendType.Email)
            entity.AddDomainNotification(new VerificationCodeByEmailSentEvent(entity));

        await _context.Information.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
