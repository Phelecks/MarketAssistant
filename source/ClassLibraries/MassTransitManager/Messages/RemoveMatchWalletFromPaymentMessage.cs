using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class RemoveMatchWalletFromPaymentMessage : IRemoveMatchWalletFromPaymentMessage
{
    public RemoveMatchWalletFromPaymentMessage(string game, long matchId, string walletAddress)
    {
        Game = game;
        MatchId = matchId;
        WalletAddress = walletAddress;
    }

    public string Game { get; }
    public long MatchId { get; }
    public string WalletAddress { get; }
}