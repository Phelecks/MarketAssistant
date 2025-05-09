﻿using IdentityHelper.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace BaseApi.Handlers;

public class SiweAuthenticationHandler(IOptionsMonitor<SiweAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder,
    TimeProvider timeProvider, IConfiguration configuration, ISiweService siweService) : AuthenticationHandler<SiweAuthenticationOptions>(options, logger, encoder)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ISiweService _siweService = siweService;

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (IsHealthCheckRequest())
            return await Task.FromResult(AuthenticateResult.NoResult());

        var claimsPrincipal = Request.HttpContext.User;
        if (AlreadyAuthenticated(claimsPrincipal)) return await Task.FromResult(AuthenticateResult.NoResult());

        string? siweToken = null;
        if(Options.SupportSignalR && Request.Path.HasValue && Request.Path.Value.Contains("/hubs") && Request.QueryString.HasValue)
            siweToken = Request.Query["access_token"];
        if(string.IsNullOrEmpty(siweToken))
        {
            if(!AuthenticationHeaderValue.TryParse(Request.Headers.Authorization, out var tokenHeaderValue))
                return await Task.FromResult(AuthenticateResult.Fail($"Missing Authorization Header: {Options.TokenHeaderName}"));
            var scheme = tokenHeaderValue.Scheme;
            if (!scheme.Equals(Options.TokenHeaderName)) return await Task.FromResult(AuthenticateResult.NoResult());
            siweToken = tokenHeaderValue.Parameter;
        }

        if(string.IsNullOrEmpty(siweToken)) return await Task.FromResult(AuthenticateResult.Fail($"Missing Authorization Header: {Options.TokenHeaderName}"));

        var securityKey = GetSecret();
        if (string.IsNullOrEmpty(securityKey))
        {
            //Log error
            return AuthenticateResult.Fail("An error occured during authentication, please try later.");
        }

        var validationResult = await _siweService.ValidateSiweAsync(siweToken, securityKey, Options.ApplicationName, Options.ValidIssuers);

        //usually, this is where you decrypt a token and/or lookup a database.
        if (!validationResult.Authorized)
            return AuthenticateResult.Fail(string.IsNullOrEmpty(validationResult.ErrorMessage) ? "An error occured during authentication, please try later." : validationResult.ErrorMessage);
        
        return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(validationResult.claims, Scheme.Name)), Scheme.Name));
    }

    bool IsHealthCheckRequest()
    {
        if (Request.Path.HasValue && (Request.Path.Value.Contains("/grpc.health.v1.Health/Check") || Request.Path.Value.Equals("/health")))
            return true;
        return false;
    }

    static bool AlreadyAuthenticated(ClaimsPrincipal? claimsPrincipal)
    { 
        if (claimsPrincipal is not null && !string.IsNullOrWhiteSpace(claimsPrincipal.Identity?.Name))
            return false;
        if (claimsPrincipal is not null && !string.IsNullOrWhiteSpace(claimsPrincipal.Identity?.Name))
            return false;
        return true;
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
    public string ApplicationName { get; set; } = string.Empty;
    public string[] ValidIssuers { get; set; } = [];
    public bool SupportSignalR { get; set; } = false;
}