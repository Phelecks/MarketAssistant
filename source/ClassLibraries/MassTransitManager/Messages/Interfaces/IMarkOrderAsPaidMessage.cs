using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface IMarkOrderAsPaidMessage : CorrelatedBy<Guid>
{
}