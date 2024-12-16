using Microsoft.AspNetCore.SignalR;

namespace Informing.Grpc.Helpers;

public class CustomSignalRUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst(IdentityModel.JwtClaimTypes.Id)?.Value!;
    }
}
