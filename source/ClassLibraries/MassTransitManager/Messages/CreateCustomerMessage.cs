using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateCustomerMessage : ICreateCustomerMessage
{
    public Guid CorrelationId { get; }

    /// <summary>
    /// User identifier
    /// </summary>
    public string UserId { get;  }

    public string ClientId { get; }

    public string? ReferralCode { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="userId">User identifier</param>
    public CreateCustomerMessage(Guid correlationId, string userId, string clientId, string? referralCode = null)
    {
        UserId = userId;
        CorrelationId = correlationId;
        ClientId = clientId;
        ReferralCode = referralCode;
    }
}