using CustomMediatR.Interfaces;

namespace SampleApi.Ping.Commands.PingWithResponse;

public record PingWithResponseCommand(string Content) : IRequest<string>;

public class Handler : IRequestHandler<PingWithResponseCommand, string>
{
    public async Task<string> HandleAsync(PingWithResponseCommand request, CancellationToken cancellationToken)
    {
        return await Task.FromResult($"{request.Content} delivered.");
    }
}