namespace MassTransitManager.Events.Interfaces;

public interface IGameInitializedEvent
{
    string Game { get; }
    float ReferralPercent { get; }
    string WithdrawWalletAddress { get; }
}