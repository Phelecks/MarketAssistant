namespace MassTransitManager.Messages.Interfaces;

public interface IRemoveMatchWalletFromPaymentMessage
{
    string Game { get; }
    long MatchId { get; }
    string WalletAddress { get; }
}