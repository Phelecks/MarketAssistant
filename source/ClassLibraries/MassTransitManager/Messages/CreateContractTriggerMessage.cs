using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateContractTriggerMessage : ICreateContractTriggerMessage
{
    public string Title { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="clientId"></param>
    public CreateContractTriggerMessage(string title)
    {
        Title = title;
    }
}