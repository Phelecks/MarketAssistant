﻿using BaseApplication.Exceptions;
using Informing.Domain.Entities;
using Informing.Domain.Events.Information;
using MediatR;
using Microsoft.EntityFrameworkCore;
using IApplicationDbContext = Informing.Application.Interfaces.IApplicationDbContext;

namespace Informing.Application.Information.Commands.SendVerificationCodeByEmail;


public record SendVerificationCodeByEmailCommand(string userId, string emailAddress, string verificationCode) : IRequest<long>;

public class SendVerificationCodeByEmailCommandHandler : IRequestHandler<SendVerificationCodeByEmailCommand, long>
{
    private readonly IApplicationDbContext _context;

    public SendVerificationCodeByEmailCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(SendVerificationCodeByEmailCommand request, CancellationToken cancellationToken)
    {
        var template = await _context.templates.SingleOrDefaultAsync(exp => exp.informingType == BaseDomain.Enums.InformingEnums.InformingType.VerifyEmailAddress && exp.informingSendType == BaseDomain.Enums.InformingEnums.InformingSendType.Email, cancellationToken);
        if (template == null) throw new NotFoundException($"Template with informingType: {BaseDomain.Enums.InformingEnums.InformingType.VerifyEmailAddress} and informingSendType: {BaseDomain.Enums.InformingEnums.InformingSendType.Email}, not found.");
        
        var content = template.content.Replace("@VerificationCode", request.verificationCode);

        var contact = await _context.contacts.SingleOrDefaultAsync(exp => exp.emailAddress != null && exp.emailAddress.Equals(request.emailAddress), cancellationToken);
        if (contact == null) throw new NotFoundException(nameof(Domain.Entities.Contact), request.emailAddress);

        var entity = new Domain.Entities.Information
        {
            content = content,
            title = template.title,
            type = InformationType.Email,
            contactInformations = new List<Domain.Entities.ContactInformation> 
            {
                new ContactInformation
                {
                    contact = contact
                }
            }
        };
        entity.AddDomainEvent(new VerificationCodeByEmailSentEvent(entity));

        await _context.information.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
