namespace MassTransitManager.Messages.Interfaces;

public interface IDeleteTokenMessage
{
    long TokenId { get; }
}