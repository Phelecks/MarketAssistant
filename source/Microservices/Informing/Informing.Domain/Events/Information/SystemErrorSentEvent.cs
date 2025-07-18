using MediatR.Interfaces;

namespace Informing.Domain.Events.Information;

public class SystemErrorSentEvent : INotification
{
    public SystemErrorSentEvent(Entities.Information item)
    {
        this.item = item;
    }

    public Entities.Information item { get; }
}
