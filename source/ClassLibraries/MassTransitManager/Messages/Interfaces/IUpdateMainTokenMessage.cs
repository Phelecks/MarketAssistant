namespace MassTransitManager.Messages.Interfaces;

public interface IUpdateMainTokenMessage
{
    long TokenId { get; }
    string Symbol { get; }
    int ChainId { get; }
}