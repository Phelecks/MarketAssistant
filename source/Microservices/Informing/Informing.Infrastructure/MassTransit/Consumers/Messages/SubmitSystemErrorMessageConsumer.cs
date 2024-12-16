using Informing.Application.Information.Commands.SendError;
using Informing.Application.Interfaces;
using MassTransit;
using MassTransitManager.Messages.Interfaces;
using MediatR;

namespace Informing.Infrastructure.MassTransit.Consumers.Messages;

public class SubmitSystemErrorMessageConsumer : IConsumer<ISubmitSystemErrorMessage>
{
    private readonly ISender _sender;
    private readonly IDiscordService _discordService;

    public SubmitSystemErrorMessageConsumer(ISender sender, IDiscordService discordService)
    {
        _sender = sender;
        _discordService = discordService;
    }
    public async Task Consume(ConsumeContext<ISubmitSystemErrorMessage> context)
    {
        //await _sender.Send(new SendErrorCommand(context.Message.Content), context.CancellationToken);
        await _discordService.SendMessageAsync(context.Message.Content, 1292911958881734756, context.CancellationToken);
    }
}