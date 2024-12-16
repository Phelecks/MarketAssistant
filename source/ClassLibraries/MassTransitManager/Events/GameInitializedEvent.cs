using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class GameInitializedEvent : IGameInitializedEvent
{
    public string Game { get; }

    public float ReferralPercent { get; }

    public string WithdrawWalletAddress { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    public GameInitializedEvent(string game, float referralPercent, string withdrawWalletAddress)
    {
        Game = game;
        ReferralPercent = referralPercent;
        WithdrawWalletAddress = withdrawWalletAddress;
    }
}