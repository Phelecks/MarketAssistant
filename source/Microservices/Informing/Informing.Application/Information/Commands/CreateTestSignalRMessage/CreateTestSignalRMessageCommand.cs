using Informing.Application.Interfaces;
using MediatR;

namespace Informing.Application.Information.Commands.CreateTestSignalRMessage;


public record CreateTestSignalRMessageCommand() : IRequest<Unit>;

public class CreateInformationCommandHandler : IRequestHandler<CreateTestSignalRMessageCommand, Unit>
{
    private readonly IGameHubProxy _gameHubProxy;
    public CreateInformationCommandHandler(IGameHubProxy gameHubProxy)
    {
        _gameHubProxy = gameHubProxy;
    }

    public async Task<Unit> Handle(CreateTestSignalRMessageCommand request, CancellationToken cancellationToken)
    {
        await _gameHubProxy.NotifyBetInitiatedAsync("0xe1BA310dC3481EE3a242B1aDDbDE4049F70784B9", 
            new Domain.SignalREntities.BetInitiatedDto("Coin", 1, Guid.NewGuid(), 1, 1, "hash", DateTime.Now, 
            new Domain.SignalREntities.BetInitiatedDto.OptionDto(1, "title", new Uri("https://www.google.com"))));

        return Unit.Value;
    }
}
