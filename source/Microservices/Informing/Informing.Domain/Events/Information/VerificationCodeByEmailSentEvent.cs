using MediatR.Interfaces;

namespace Informing.Domain.Events.Information;

public class VerificationCodeByEmailSentEvent : INotification
{
    public VerificationCodeByEmailSentEvent(Entities.Information item)
    {
        this.item = item;
    }

    public Entities.Information item { get; }
}
