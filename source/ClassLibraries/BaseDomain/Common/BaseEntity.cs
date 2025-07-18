using System.ComponentModel.DataAnnotations.Schema;
using MediatR.Interfaces;

namespace BaseDomain.Common;

public abstract class BaseEntity
{
    public long Id { get; set; }

    private readonly List<INotification> _domainNotifications = new();

    [NotMapped]
    public IReadOnlyCollection<INotification> DomainNotifications => _domainNotifications.AsReadOnly();

    public void AddDomainNotification(INotification domainNotification)
    {
        _domainNotifications.Add(domainNotification);
    }

    public void RemoveDomainNotification(INotification domainNotification)
    {
        _domainNotifications.Remove(domainNotification);
    }

    public void ClearDomainNotifications()
    {
        _domainNotifications.Clear();
    }
}
