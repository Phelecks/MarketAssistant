using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface ICreateFinancialAccountMessage : CorrelatedBy<Guid>
{
    /// <summary>
    /// User identifier
    /// </summary>
    string UserId { get; }
}