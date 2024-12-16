using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateResourceMessage : ICreateResourceMessage
{
    public string Title { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="clientId"></param>
    public CreateResourceMessage(string title)
    {
        Title = title;
    }
}