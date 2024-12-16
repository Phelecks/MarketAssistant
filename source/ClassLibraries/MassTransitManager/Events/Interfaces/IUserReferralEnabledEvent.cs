using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IUserReferralEnabledEvent : CorrelatedBy<Guid>
{
}