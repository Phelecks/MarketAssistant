using Discord;
using Discord.WebSocket;
using Informing.Application.Interfaces;
using Informing.Infrastructure.Helpers;
using LoggerService.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Informing.Infrastructure.Services;

public class DiscordService : IDiscordService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DiscordSocketClient _discord;
    private readonly ILogger<DiscordSocketClient> _logger;
    private bool Initialized = false;
    private readonly Random _random;

    public DiscordService(IServiceProvider serviceProvider, DiscordSocketClient discord, ILogger<DiscordSocketClient> logger)
    {
        _serviceProvider = serviceProvider;
        _discord = discord;
        _logger = logger;
        _discord.Log += msg => DiscordLogHelper.OnLogAsync(_logger, msg);
        _random = new Random();
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var botToken = await context.baseParameters.SingleOrDefaultAsync(exp => exp.field == BaseDomain.Enums.BaseParameterField.InformingDiscordBotToken, cancellationToken);
        if(botToken is null)
        {
            Initialized = false;
            return;
        }

        await _discord.LoginAsync(TokenType.Bot, botToken.value);
        await _discord.StartAsync();
        Initialized = true;
    }

    public async Task SendMessageAsync(string message, ulong channelId, CancellationToken cancellationToken)
    {
        try
        {
            if (!Initialized)
            {
                //Try initialize
                await InitializeAsync(cancellationToken);
                if (!Initialized)
                    return;
            }

            var channel = await _discord.GetChannelAsync(channelId) as IMessageChannel;
            await channel.SendMessageAsync(text: message);
        }
        catch (Exception exception)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.InformingException, eventName: "Exception"),
                exception: exception, message: exception.Message), cancellationToken);
            return;
        }
    }

    public async Task SendBetMessageAsync(ulong channelId, string game, long matchId, string userWalletAddress, string userWalletAddressUrl, string formattedValue, string transactionHash, string transactionUrl, string optionTitle, string optionThumbnailUrl, DateTime dateTime, CancellationToken cancellationToken)
    {
        try
        {
            if (!Initialized)
            {
                //Try initialize
                await InitializeAsync(cancellationToken);
                if (!Initialized)
                    return;
            }

            var embedBuilder = new EmbedBuilder()
                .WithTitle("New Prediction")
                .WithDescription(GetBetDescription())
                .AddField("Match ID", matchId)
                .AddField("Wallet Address", $"[{userWalletAddress}]({userWalletAddressUrl})")
                .AddField("Value", formattedValue)
                .AddField("Transaction", $"[{transactionHash}]({transactionUrl})")
                .AddField("Date & Time", dateTime.ToString("MMMM dd, yyyy - hh:mm:ss tt"))
                .AddField("Option", $"{DiscordEmojiHelper.GetEmoji(game, optionTitle)} {optionTitle}")
                .WithThumbnailUrl(optionThumbnailUrl)
                .WithColor(Color.Blue)
                .WithFooter(footer => footer.Text = "by Tricksfor")
                .WithCurrentTimestamp();
            var embed = embedBuilder.Build();

            var channel = await _discord.GetChannelAsync(channelId) as IMessageChannel;
            await channel.SendMessageAsync(embed: embed);
        }
        catch (Exception exception)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.InformingException, eventName: "Exception"),
                exception: exception, message: exception.Message), cancellationToken);
            return;
        }
    }

    public async Task SendRewardMessageAsync(ulong channelId, string game, long matchId, string userWalletAddress, string userWalletAddressUrl, string formattedValue, string transactionHash, string transactionUrl, DateTime dateTime, CancellationToken cancellationToken)
    {
        try
        {
            if (!Initialized)
            {
                //Try initialize
                await InitializeAsync(cancellationToken);
                if (!Initialized)
                    return;
            }

            var embedBuilder = new EmbedBuilder()
                .WithTitle("Reward Paid")
                .WithDescription(GetRewardDescription())
                .AddField("Match ID", matchId)
                .AddField("Wallet Address", $"[{userWalletAddress}]({userWalletAddressUrl})")
                .AddField("Value", formattedValue)
                .AddField("Transaction", $"[{transactionHash}]({transactionUrl})")
                .AddField("Date & Time", dateTime.ToString("MMMM dd, yyyy - hh:mm:ss tt"))
                .WithColor(Color.Green)
                .WithFooter(footer => footer.Text = "by Tricksfor")
                .WithCurrentTimestamp();
            var embed = embedBuilder.Build();

            var channel = await _discord.GetChannelAsync(channelId) as IMessageChannel;
            await channel.SendMessageAsync(embed: embed);
        }
        catch (Exception exception)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.InformingException, eventName: "Exception"),
                exception: exception, message: exception.Message), cancellationToken);
            return;
        }
    }

    public async Task SendReferralRewardMessageAsync(ulong channelId, string game, long matchId, string userWalletAddress, string userWalletAddressUrl, string formattedValue, string transactionHash, string transactionUrl, DateTime dateTime, CancellationToken cancellationToken)
    {
        try
        {
            if (!Initialized)
            {
                //Try initialize
                await InitializeAsync(cancellationToken);
                if (!Initialized)
                    return;
            }

            var embedBuilder = new EmbedBuilder()
                .WithTitle("Referral Reward Paid")
                .WithDescription(GetReferralRewardDescription())
                .AddField("Match ID", matchId)
                .AddField("Wallet Address", $"[{userWalletAddress}]({userWalletAddressUrl})")
                .AddField("Value", formattedValue)
                .AddField("Transaction", $"[{transactionHash}]({transactionUrl})")
                .AddField("Date & Time", dateTime.ToString("MMMM dd, yyyy - hh:mm:ss tt"))
                .WithColor(Color.Gold)
                .WithFooter(footer => footer.Text = "by Tricksfor")
                .WithCurrentTimestamp();
            var embed = embedBuilder.Build();

            var channel = await _discord.GetChannelAsync(channelId) as IMessageChannel;
            await channel.SendMessageAsync(embed: embed);
        }
        catch (Exception exception)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.InformingException, eventName: "Exception"),
                exception: exception, message: exception.Message), cancellationToken);
            return;
        }
    }

    public async Task SendWinStatusUpdateMessageAsync(ulong channelId, string game, long matchId, string userWalletAddress, string userWalletAddressUrl, string formattedValue, string transactionHash, string transactionUrl, string optionTitle, string optionThumbnailUrl, CancellationToken cancellationToken)
    {
        try
        {
            if (!Initialized)
            {
                //Try initialize
                await InitializeAsync(cancellationToken);
                if (!Initialized)
                    return;
            }

            var embedBuilder = new EmbedBuilder()
                .WithTitle("WIN")
                .AddField("Match ID", matchId)
                .AddField("Wallet Address", $"[{userWalletAddress}]({userWalletAddressUrl})")
                //.AddField("Value", formattedValue)
                .AddField("Transaction", $"[{transactionHash}]({transactionUrl})")
                .AddField("Option", $"{DiscordEmojiHelper.GetEmoji(game, optionTitle)} {optionTitle}")
                .WithThumbnailUrl(optionThumbnailUrl)
                .WithColor(Color.DarkGreen)
                .WithFooter(footer => footer.Text = "by Tricksfor")
                .WithCurrentTimestamp();
            var embed = embedBuilder.Build();

            var channel = await _discord.GetChannelAsync(channelId) as IMessageChannel;
            await channel.SendMessageAsync(embed: embed);
        }
        catch (Exception exception)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.InformingException, eventName: "Exception"),
                exception: exception, message: exception.Message), cancellationToken);
            return;
        }
    }

    public async Task SendLoseStatusUpdateMessageAsync(ulong channelId, string game, long matchId, string userWalletAddress, string userWalletAddressUrl, string formattedValue, string transactionHash, string transactionUrl, string optionTitle, string optionThumbnailUrl, CancellationToken cancellationToken)
    {
        try
        {
            if (!Initialized)
            {
                //Try initialize
                await InitializeAsync(cancellationToken);
                if (!Initialized)
                    return;
            }

            var embedBuilder = new EmbedBuilder()
                .WithTitle("LOSE")
                .AddField("Match ID", matchId)
                .AddField("Wallet Address", $"[{userWalletAddress}]({userWalletAddressUrl})")
                //.AddField("Value", formattedValue)
                .AddField("Transaction", $"[{transactionHash}]({transactionUrl})")
                .AddField("Option", $"{DiscordEmojiHelper.GetEmoji(game, optionTitle)} {optionTitle}")
                .WithThumbnailUrl(optionThumbnailUrl)
                .WithColor(Color.DarkRed)
                .WithFooter(footer => footer.Text = "by Tricksfor")
                .WithCurrentTimestamp();
            var embed = embedBuilder.Build();

            var channel = await _discord.GetChannelAsync(channelId) as IMessageChannel;
            await channel.SendMessageAsync(embed: embed);
        }
        catch (Exception exception)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.InformingException, eventName: "Exception"),
                exception: exception, message: exception.Message), cancellationToken);
            return;
        }
    }

    public async Task SendDrawStatusUpdateMessageAsync(ulong channelId, string game, long matchId, string userWalletAddress, string userWalletAddressUrl, string formattedValue, string transactionHash, string transactionUrl, string optionTitle, string optionThumbnailUrl, CancellationToken cancellationToken)
    {
        try
        {
            if (!Initialized)
            {
                //Try initialize
                await InitializeAsync(cancellationToken);
                if (!Initialized)
                    return;
            }

            var embedBuilder = new EmbedBuilder()
                .WithTitle("DRAW")
                .AddField("Match ID", matchId)
                .AddField("Wallet Address", $"[{userWalletAddress}]({userWalletAddressUrl})")
                //.AddField("Value", formattedValue)
                .AddField("Transaction", $"[{transactionHash}]({transactionUrl})")
                .AddField("Option", $"{DiscordEmojiHelper.GetEmoji(game, optionTitle)} {optionTitle}")
                .WithThumbnailUrl(optionThumbnailUrl)
                .WithColor(Color.DarkGrey)
                .WithFooter(footer => footer.Text = "by Tricksfor")
                .WithCurrentTimestamp();
            var embed = embedBuilder.Build();

            var channel = await _discord.GetChannelAsync(channelId) as IMessageChannel;
            await channel.SendMessageAsync(embed: embed);
        }
        catch (Exception exception)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.InformingException, eventName: "Exception"),
                exception: exception, message: exception.Message), cancellationToken);
            return;
        }
    }

    public async Task SendMatchOverviewMessageAsync(ulong channelId, string game, long matchId, string matchWalletAddress, string matchWalletAddressUrl, double minutesLeft, string ImageUrl, int bets, string formattedBetValue, CancellationToken cancellationToken)
    {
        try
        {
            if (!Initialized)
            {
                //Try initialize
                await InitializeAsync(cancellationToken);
                if (!Initialized)
                    return;
            }

            var embedBuilder = new EmbedBuilder()
                .WithTitle("Match Overview")
                .WithDescription("We'll keep you updated")
                .AddField("Match ID", matchId)
                .AddField("Wallet Address", $"[{matchWalletAddress}]({matchWalletAddressUrl})")
                .AddField("Number of predictions", bets)
                .AddField("Total prediction value", formattedBetValue)
                .AddField("Times left", GetTimesLeft(minutesLeft))
                .WithImageUrl(ImageUrl)
                .WithColor(Color.Purple)
                .WithFooter(footer => footer.Text = "by Tricksfor")
                .WithCurrentTimestamp();
            var embed = embedBuilder.Build();

            var channel = await _discord.GetChannelAsync(channelId) as IMessageChannel;
            await channel.SendMessageAsync(embed: embed);
        }
        catch (Exception exception)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.InformingException, eventName: "Exception"),
                exception: exception, message: exception.Message), cancellationToken);
            return;
        }
    }

    public async Task SendMatchMinutesLeftMessageAsync(ulong channelId, string game, long matchId, string matchWalletAddress, string matchWalletAddressUrl, double minutesLeft, string ImageUrl, CancellationToken cancellationToken)
    {
        try
        {
            if (!Initialized)
            {
                //Try initialize
                await InitializeAsync(cancellationToken);
                if (!Initialized)
                    return;
            }

            var embedBuilder = new EmbedBuilder()
                .WithTitle($"Only {minutesLeft} Minutes Left")
                .AddField("Match ID", matchId)
                .AddField("Wallet Address", $"[{matchWalletAddress}]({matchWalletAddressUrl})")
                .WithDescription("Don't miss your chance to win")
                .WithImageUrl(ImageUrl)
                .WithColor(Color.Purple)
                .WithFooter(footer => footer.Text = "by Tricksfor")
                .WithCurrentTimestamp();
            var embed = embedBuilder.Build();

            var channel = await _discord.GetChannelAsync(channelId) as IMessageChannel;
            await channel.SendMessageAsync(embed: embed);
        }
        catch (Exception exception)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.InformingException, eventName: "Exception"),
                exception: exception, message: exception.Message), cancellationToken);
            return;
        }
    }





    private string GetTimesLeft(double minutesLeft)
    {
        var hoursLeft = (int)(minutesLeft / 60);

        if (hoursLeft <= 1)
            return "About an hour to the end of this match";
        else
            return $"About {hoursLeft} hours to the end of this match";
    }
    private string GetBetDescription()
    {
        var messages = new List<string>()
        {
            "Maybe it's the winner after all",
            "Here's a guess, maybe it's the winner",
            "It may be the winner, guess what?",
            "Who knows, maybe it will win",
            "What do you think? Maybe the winner is this one",
            "Guess what? It looks like it might have the winning ticket!"
        };
        var span = new Span<string>([.. messages]);
        _random.Shuffle<string>(span);
        return span.ToArray().First();
    }
    private string GetRewardDescription()
    {
        var messages = new List<string>()
        {
            "You've been rewarded; how awesome is that? Best of luck in the upcoming games!",
            "You earned your rewards; isn't it fantastic? May your next matches be just as lucky!",
            "You've been recognized; isn't it thrilling? Wishing you all the best in your future games!",
            "You received your rewards; isn't that incredible? Good fortune for your next matches!",
            "You've earned your prize; isn't it wonderful? Good luck with the next challenges!",
            "Rewarded; how cool! Best wishes for the next matches!",
            "You earned rewards; fantastic! Good luck ahead.",
            "You were rewarded; incredible! All the best in the next games."
        };
        var span = new Span<string>([.. messages]);
        _random.Shuffle<string>(span);
        return span.ToArray().First();
    }
    private string GetReferralRewardDescription()
    {
        var messages = new List<string>()
        {
            "Your referrals hit the jackpot; what amazing fortune!",
            "Your recommenders just struck gold; talk about lucky breaks!",
            "Those who referred you just reaped their rewards; this is the height of good luck!",
            "Your references are celebrating their rewards—what incredible luck!",
            "The people who vouched for you were rewarded handsomely; such fortuitous success!",
            "Your referees just struck it rich; couldn't have had better luck!",
            "Your champions are basking in their rewards—what serendipity!",
            "Your supporters have been handsomely rewarded; sheer good fortune!"
        };
        var span = new Span<string>([.. messages]);
        _random.Shuffle<string>(span);
        return span.ToArray().First();
    }
}
