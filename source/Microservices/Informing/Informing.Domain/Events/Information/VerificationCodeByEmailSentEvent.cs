using BaseDomain.Common;

namespace Informing.Domain.Events.Information;

public class VerificationCodeByEmailSentEvent : BaseEvent
{
    public VerificationCodeByEmailSentEvent(Entities.Information item)
    {
        this.item = item;
    }

    public Entities.Information item { get; }
}
