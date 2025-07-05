using CustomMediatR.Interfaces;

namespace SampleApi.Ping.Commands;

public record PingCommand(string Content) : IRequest<string>;

public class Handler : IRequestHandler<PingCommand, string>
{
    public async Task<string> HandleAsync(PingCommand request, CancellationToken cancellationToken)
    {
        return await Task.FromResult($"{request.Content} delivered.");
    }
}