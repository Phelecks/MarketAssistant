using Informing.Application.Contact.Commands.CreateContact;
using MassTransit;
using MassTransitManager.Events;
using MassTransitManager.Messages.Interfaces;
using MassTransitManager.Services;
using MediatR.Interfaces;

namespace Informing.Infrastructure.MassTransit.Consumers.Messages;

public class CreateInformingContactMessageConsumer : IConsumer<ICreateInformingContactMessage>
{
    private readonly IRequestDispatcher _dispatcher;
    private readonly IMassTransitService _massTransitService;

    public CreateInformingContactMessageConsumer(IRequestDispatcher dispatcher, IMassTransitService massTransitService)
    {
        _dispatcher = dispatcher;
        _massTransitService = massTransitService;
    }
    public async Task Consume(ConsumeContext<ICreateInformingContactMessage> context)
    {
        try
        {
            var result = await _dispatcher.SendAsync(new CreateContactCommand(context.Message.UserId, context.Message.Username,
                context.Message.PhoneNumber, context.Message.EmailAddress), context.CancellationToken);
            await _massTransitService.PublishAsync(new InformingContactCreatedEvent(context.Message.CorrelationId, result));
        }
        catch (Exception e)
        {
            await _massTransitService.PublishAsync(new CreateInformingContactFailedEvent(context.Message.CorrelationId,
                e.Message));
        }

    }
}