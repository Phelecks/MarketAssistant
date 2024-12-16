namespace Informing.Application.Interfaces;

public interface IContactService
{
    Task<Domain.Entities.Contact> FindContactAsync(string destination, CancellationToken cancellationToken);
}