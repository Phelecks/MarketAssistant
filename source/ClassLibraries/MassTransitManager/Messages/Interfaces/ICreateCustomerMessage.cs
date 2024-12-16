using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface ICreateCustomerMessage : CorrelatedBy<Guid>
{
    /// <summary>
    /// User identifier
    /// </summary>
    string UserId { get; }

    string ClientId { get; }

    string? ReferralCode { get; }
}