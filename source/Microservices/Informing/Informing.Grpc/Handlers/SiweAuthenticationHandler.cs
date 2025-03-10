using IdentityHelper.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Informing.Grpc.Handlers;

public class SiweAuthenticationHandler : AuthenticationHandler<SiweAuthenticationOptions>
{
    private readonly IConfiguration _configuration;
    private readonly ISiweService _siweService;

    public SiweAuthenticationHandler(IOptionsMonitor<SiweAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder,
        TimeProvider timeProvider, IConfiguration configuration, ISiweService siweService) : base(options, logger, encoder)
    {
        _configuration = configuration;
        _siweService = siweService;
    }
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Request.Path.HasValue && (Request.Path.Value.Contains("/grpc.health.v1.Health/Check") || Request.Path.Value.Equals("/health")))
            return await Task.FromResult(AuthenticateResult.NoResult());

        var user = Request.HttpContext.User;
        if (user is not null && user.Identity is not null && user.Identity.IsAuthenticated) return await Task.FromResult(AuthenticateResult.NoResult());

        if (user is not null && !string.IsNullOrWhiteSpace(user.Identity?.Name))
            return await Task.FromResult(AuthenticateResult.NoResult()); //Already authenticated 

        string? siweToken = null;
        if(Request.Path.HasValue && Request.Path.Value.Contains("/hubs") && Request.QueryString.HasValue)
            siweToken = Request.Query["access_token"];
        if(string.IsNullOrEmpty(siweToken))
        {
            if(!AuthenticationHeaderValue.TryParse(Request.Headers.Authorization, out var tokenHeaderValue))
                return await Task.FromResult(AuthenticateResult.Fail($"Missing Authorization Header: {Options.TokenHeaderName}"));
            var scheme = tokenHeaderValue.Scheme;
            if (!scheme.Equals("Bearer")) return await Task.FromResult(AuthenticateResult.NoResult());
            siweToken = tokenHeaderValue.Parameter;
        }

        if(string.IsNullOrEmpty(siweToken)) return await Task.FromResult(AuthenticateResult.Fail($"Missing Authorization Header: {Options.TokenHeaderName}"));

        var securityKey = GetSecret();
        if (string.IsNullOrEmpty(securityKey))
            return AuthenticateResult.Fail("An error occured during authentication, please try later.");
        
        var validationResult = await _siweService.ValidateSiweAsync(siweToken, securityKey, Options.ApplicationName, Options.ValidIssuers);

        if (!validationResult.Authorized)
            return AuthenticateResult.Fail(string.IsNullOrEmpty(validationResult.ErrorMessage) ? "An error occured during authentication, please try later." : validationResult.ErrorMessage);
        
        return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(validationResult.claims, Scheme.Name)), Scheme.Name));
    }

    string GetSecret()
    {
        return _configuration.GetValue("IDENTITY-SECRET", "DefaultSecretValue123")!;
    }
}


public class SiweAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "Siwe";
    public string TokenHeaderName { get; set; } = "Siwe";
    public string ApplicationName { get; set; }
    public string[] ValidIssuers { get; set; }
}