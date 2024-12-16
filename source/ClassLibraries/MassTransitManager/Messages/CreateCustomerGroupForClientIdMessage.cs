using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateCustomerGroupForClientIdMessage : ICreateCustomerGroupForClientIdMessage
{
    public string ClientId { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="clientId"></param>
    public CreateCustomerGroupForClientIdMessage(string clientId)
    {
        ClientId = clientId;
    }
}