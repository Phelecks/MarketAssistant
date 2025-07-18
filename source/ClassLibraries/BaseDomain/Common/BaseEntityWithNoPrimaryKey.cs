using System.ComponentModel.DataAnnotations.Schema;
using MediatR.Interfaces;

namespace BaseDomain.Common;

public abstract class BaseEntityWithNoPrimaryKey
{
    private readonly List<INotification> _domainNotification = new();

    [NotMapped]
    public IReadOnlyCollection<INotification> DomainNotifications => _domainNotification.AsReadOnly();

    public void AddDomainNotification(INotification domainNotification)
    {
        _domainNotification.Add(domainNotification);
    }

    public void RemoveDomainNotification(INotification domainNotification)
    {
        _domainNotification.Remove(domainNotification);
    }

    public void ClearDomainNotifications()
    {
        _domainNotification.Clear();
    }
}
