using Informing.Application.Contact.Commands.UpdateContactByUserId;
using MassTransit;
using MassTransitManager.Messages.Interfaces;
using MediatR;

namespace Informing.Infrastructure.MassTransit.Consumers.Messages;

public class UpdateInformingContactMessageConsumer : IConsumer<IUpdateInformingContactMessage>
{
    private readonly ISender _sender;

    public UpdateInformingContactMessageConsumer(ISender sender)
    {
        _sender = sender;
    }
    public async Task Consume(ConsumeContext<IUpdateInformingContactMessage> context)
    {
        await _sender.Send(new UpdateContactByUserIdCommand(context.Message.UserId, context.Message.EmailAddress,
            context.Message.PhoneNumber, context.Message.Fullname));
    }
}