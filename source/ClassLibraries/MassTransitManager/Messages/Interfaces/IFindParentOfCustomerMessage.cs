using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface IFindParentOfCustomerMessage : CorrelatedBy<Guid>
{
    string UserId { get; }
}