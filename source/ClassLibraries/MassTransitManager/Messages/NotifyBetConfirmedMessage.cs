using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class NotifyBetConfirmedMessage(NotifyBetConfirmedMessage.MatchDto match, 
    NotifyBetConfirmedMessage.TransactionDto transaction, 
    INotifyBetConfirmedMessage.OptionDto option, string userId, INotifyBetConfirmedMessage.DiscordMessageDto? discordMessage) : INotifyBetConfirmedMessage
{
    public string Game { get; } = match.Game;
    public long MatchId { get; } = match.MatchId;
    public Guid Id { get; } = transaction.Id;
    public decimal Value { get; } = transaction.Value;
    public long ExternalTokenId { get; } = transaction.ExternalTokenId;
    public string TransactionHash { get; } = transaction.TransactionHash;
    public DateTime DateTime { get; } = transaction.DateTime;
    public INotifyBetConfirmedMessage.OptionDto Option { get; } = option;
    public string UserId { get; } = userId;
    public INotifyBetConfirmedMessage.DiscordMessageDto? DiscordMessage { get; } = discordMessage;

    public record MatchDto(string Game, long MatchId);
    public record TransactionDto(Guid Id, decimal Value, long ExternalTokenId, string TransactionHash, DateTime DateTime);
}
