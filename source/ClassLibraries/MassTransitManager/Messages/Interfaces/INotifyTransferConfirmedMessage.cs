
namespace MassTransitManager.Messages.Interfaces;

public interface INotifyTransferConfirmedMessage : Events.Interfaces.ITransferConfirmedEvent
{
    string UserId { get; }
}