using CustomMediatR.Helpers;
using CustomMediatR.Interfaces;

namespace SampleApi.Ping.Commands.PingWithoutResponse;

public record PingWithoutResponseCommand(string Content) : IRequest<Unit>;

public class Handler : IRequestHandler<PingWithoutResponseCommand, Unit>
{
    public async Task<Unit> HandleAsync(PingWithoutResponseCommand request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(Unit.Value);
    }
}