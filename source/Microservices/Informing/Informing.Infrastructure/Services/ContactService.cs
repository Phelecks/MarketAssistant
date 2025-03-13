using BaseApplication.Exceptions;
using Informing.Application.Helpers;
using Informing.Application.Interfaces;
using Informing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Informing.Infrastructure.Services;

public class ContactService : IContactService
{
    private readonly IApplicationDbContext _context;

    public ContactService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Contact> FindContactAsync(string destination, CancellationToken cancellationToken)
    {
        var destinationType = DestinationHelper.GetDestionationType(destination);

        Contact? contact;

        switch (destinationType)
        {
            case DestinationHelper.DestinationType.EmailAddress:
                contact = await _context.Contacts.SingleOrDefaultAsync(exp => exp.EmailAddress != null && exp.EmailAddress.Equals(destination), cancellationToken);
                break;
            case DestinationHelper.DestinationType.PhoneNumber:
                contact = await _context.Contacts.SingleOrDefaultAsync(exp => exp.PhoneNumber != null && exp.PhoneNumber.Equals(destination), cancellationToken);
                break;
            default:
                break;
        }


        throw new NotFoundException(nameof(Contact), destination);
    }
}