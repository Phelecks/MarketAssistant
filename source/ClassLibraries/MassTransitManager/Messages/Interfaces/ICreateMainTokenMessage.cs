namespace MassTransitManager.Messages.Interfaces;

public interface ICreateMainTokenMessage
{
    long TokenId { get; }
    string Symbol { get; }
    int ChainId { get; }
    int Decimals { get; }
}