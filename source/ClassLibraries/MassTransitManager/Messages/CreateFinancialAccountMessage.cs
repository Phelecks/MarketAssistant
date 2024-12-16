using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateFinancialAccountMessage : ICreateFinancialAccountMessage
{
    public Guid CorrelationId { get; }

    /// <summary>
    /// User identifier
    /// </summary>
    public string UserId { get;  }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="userId">User identifier</param>
    public CreateFinancialAccountMessage(Guid correlationId, string userId)
    {
        UserId = userId;
        CorrelationId = correlationId;
    }
}