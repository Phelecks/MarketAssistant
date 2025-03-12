using System.Security.Claims;

namespace IdentityHelper.Services;

public interface ISiweService
{
    Task<ValidationResponse> ValidateSiweAsync(string token, string securityKey, string applicationName, string[] validIssuers);

    public record ValidationResponse(bool Authorized, IEnumerable<Claim>? claims, string? ErrorMessage);
}