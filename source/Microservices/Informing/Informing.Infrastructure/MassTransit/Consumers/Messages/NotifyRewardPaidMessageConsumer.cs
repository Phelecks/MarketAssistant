using Informing.Application.Interfaces;
using MassTransit;
using MassTransitManager.Messages.Interfaces;

namespace Informing.Infrastructure.MassTransit.Consumers.Messages;

public class NotifyRewardPaidMessageConsumer : IConsumer<INotifyRewardPaidMessage>
{
    private readonly IDiscordService _discordService;

    public NotifyRewardPaidMessageConsumer(IDiscordService discordService)
    {
        _discordService = discordService;
    }
    public async Task Consume(ConsumeContext<INotifyRewardPaidMessage> context)
    {
        if (context.Message.DiscordMessage is not null)
        {
            switch (context.Message.Type)
            {
                case INotifyRewardPaidMessage.RewardType.WinReward:
                case INotifyRewardPaidMessage.RewardType.DrawReward:
                    await _discordService.SendRewardMessageAsync(context.Message.DiscordMessage.ChannelId, context.Message.DiscordMessage.Game, context.Message.DiscordMessage.MatchId, context.Message.DiscordMessage.UserWalletAddress, context.Message.DiscordMessage.UserWalletAddressUrl, context.Message.DiscordMessage.FormattedValue, context.Message.DiscordMessage.TransactionHash, context.Message.DiscordMessage.TransactionUrl, context.Message.DateTime, context.CancellationToken);
                    break;
                case INotifyRewardPaidMessage.RewardType.ReferralReward:
                    await _discordService.SendReferralRewardMessageAsync(context.Message.DiscordMessage.ChannelId, context.Message.DiscordMessage.Game, context.Message.DiscordMessage.MatchId, context.Message.DiscordMessage.UserWalletAddress, context.Message.DiscordMessage.UserWalletAddressUrl, context.Message.DiscordMessage.FormattedValue, context.Message.DiscordMessage.TransactionHash, context.Message.DiscordMessage.TransactionUrl, context.Message.DateTime, context.CancellationToken);
                    break;
                default:
                    break;
            }
        }
            
    }
}