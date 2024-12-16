using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class SubmitSystemErrorMessage : ISubmitSystemErrorMessage
{
    public SubmitSystemErrorMessage(string content)
    {
        Content = content;
    }

    public string Content { get; }
}