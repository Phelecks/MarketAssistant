using Informing.Application.Contact.Commands.UpdateContactByUserId;
using MassTransit;
using MassTransitManager.Messages.Interfaces;
using MediatR.Interfaces;

namespace Informing.Infrastructure.MassTransit.Consumers.Messages;

public class UpdateInformingContactMessageConsumer : IConsumer<IUpdateInformingContactMessage>
{
    private readonly IRequestDispatcher _dispatcher;

    public UpdateInformingContactMessageConsumer(IRequestDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
    public async Task Consume(ConsumeContext<IUpdateInformingContactMessage> context)
    {
        await _dispatcher.SendAsync(new UpdateContactByUserIdCommand(context.Message.UserId, context.Message.EmailAddress,
            context.Message.PhoneNumber, context.Message.Fullname), context.CancellationToken);
    }
}