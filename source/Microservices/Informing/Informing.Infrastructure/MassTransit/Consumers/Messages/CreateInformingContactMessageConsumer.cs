using Informing.Application.Contact.Commands.CreateContact;
using MassTransit;
using MassTransitManager.Events;
using MassTransitManager.Messages.Interfaces;
using MassTransitManager.Services;
using MediatR;

namespace Informing.Infrastructure.MassTransit.Consumers.Messages;

public class CreateInformingContactMessageConsumer : IConsumer<ICreateInformingContactMessage>
{
    private readonly ISender _sender;
    private readonly IMassTransitService _massTransitService;

    public CreateInformingContactMessageConsumer(ISender sender, IMassTransitService massTransitService)
    {
        _sender = sender;
        _massTransitService = massTransitService;
    }
    public async Task Consume(ConsumeContext<ICreateInformingContactMessage> context)
    {
        try
        {
            var result = await _sender.Send(new CreateContactCommand(context.Message.UserId, context.Message.Username,
                context.Message.PhoneNumber, context.Message.EmailAddress));
            await _massTransitService.PublishAsync(new InformingContactCreatedEvent(context.Message.CorrelationId, result));
        }
        catch (Exception e)
        {
            await _massTransitService.PublishAsync(new CreateInformingContactFailedEvent(context.Message.CorrelationId,
                e.Message));
        }

    }
}