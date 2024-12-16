namespace Informing.Application.Interfaces;

public interface IDiscordService
{
    Task InitializeAsync(CancellationToken cancellationToken);
    Task SendMessageAsync(string message, ulong channelId, CancellationToken cancellationToken);
    Task SendBetMessageAsync(ulong channelId, string game, long matchId, string userWalletAddress, string userWalletAddressUrl, string formattedValue, string transactionHash, string transactionUrl, string optionTitle, string optionThumbnailUrl, DateTime dateTime, CancellationToken cancellationToken);
    Task SendRewardMessageAsync(ulong channelId, string game, long matchId, string userWalletAddress, string userWalletAddressUrl, string formattedValue, string transactionHash, string transactionUrl, DateTime dateTime, CancellationToken cancellationToken);
    Task SendReferralRewardMessageAsync(ulong channelId, string game, long matchId, string userWalletAddress, string userWalletAddressUrl, string formattedValue, string transactionHash, string transactionUrl, DateTime dateTime, CancellationToken cancellationToken);
    Task SendWinStatusUpdateMessageAsync(ulong channelId, string game, long matchId, string userWalletAddress, string userWalletAddressUrl, string formattedValue, string transactionHash, string transactionUrl, string optionTitle, string optionThumbnailUrl, CancellationToken cancellationToken);
    Task SendLoseStatusUpdateMessageAsync(ulong channelId, string game, long matchId, string userWalletAddress, string userWalletAddressUrl, string formattedValue, string transactionHash, string transactionUrl, string optionTitle, string optionThumbnailUrl, CancellationToken cancellationToken);
    Task SendDrawStatusUpdateMessageAsync(ulong channelId, string game, long matchId, string userWalletAddress, string userWalletAddressUrl, string formattedValue, string transactionHash, string transactionUrl, string optionTitle, string optionThumbnailUrl, CancellationToken cancellationToken);
    Task SendMatchOverviewMessageAsync(ulong channelId, string game, long matchId, string matchWalletAddress, string matchWalletAddressUrl, double minutesLeft, string ImageUrl, int bets, string formattedBetValue, CancellationToken cancellationToken);
    Task SendMatchMinutesLeftMessageAsync(ulong channelId, string game, long matchId, string matchWalletAddress, string matchWalletAddressUrl, double minutesLeft, string ImageUrl, CancellationToken cancellationToken);
}
