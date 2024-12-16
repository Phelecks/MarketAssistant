﻿using CacheManager;
using IdentityHelper.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace BlockChainIdentity.Grpc.Handlers;

public class SiweAuthenticationHandler : AuthenticationHandler<SiweAuthenticationOptions>
{
    private readonly IDistributedCache _distributedCache;
    private readonly ISiweService _siweService;
    private readonly ILogger<SiweAuthenticationHandler> _logger;

    public SiweAuthenticationHandler(IOptionsMonitor<SiweAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder,
        TimeProvider timeProvider, IDistributedCache distributedCache, ISiweService siweService) : base(options, logger, encoder)
    {
        _distributedCache = distributedCache;
        _siweService = siweService;
        _logger = logger.CreateLogger<SiweAuthenticationHandler>();
    }
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Request.Path.HasValue && Request.Path.Value.Contains("/grpc.health.v1.Health/Check"))
            return await Task.FromResult(AuthenticateResult.NoResult());

        var user = Request.HttpContext.User;
        if (user is not null && user.Identity is not null && user.Identity.IsAuthenticated) return await Task.FromResult(AuthenticateResult.NoResult());

        if (user is not null && !string.IsNullOrWhiteSpace(user.Identity?.Name))
            return await Task.FromResult(AuthenticateResult.NoResult()); //Already authenticated 

        if (!Request.Headers.ContainsKey("Authorization"))
            return await Task.FromResult(AuthenticateResult.Fail($"Missing Authorization Header: {Options.TokenHeaderName}"));
        var tokenHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
        var scheme = tokenHeaderValue.Scheme;
        if (!scheme.Equals(Options.TokenHeaderName)) return await Task.FromResult(AuthenticateResult.NoResult());
        var siweToken = tokenHeaderValue.Parameter;

        var cacheResult = await _distributedCache.GetAsync(CacheManager.Helpers.CacheKeys.BlockChainIdentitySecret);
        var securityKey = System.Text.Json.JsonSerializer.Deserialize<string>(cacheResult);
        if (string.IsNullOrEmpty(securityKey))
        {
            //Log error
            return AuthenticateResult.Fail("An error occured during authentication, please try later.");
        }

        var validationResult = await _siweService.ValidateSiweAsync(siweToken, securityKey, Options.ApplicationName, Options.ValidIssuers);

        //usually, this is where you decrypt a token and/or lookup a database.
        if (!validationResult.Authorized)
        {
            return AuthenticateResult.Fail(string.IsNullOrEmpty(validationResult.ErrorMessage) ? "An error occured during authentication, please try later." : validationResult.ErrorMessage);
        }
        //Success! Add details here that identifies the user
        var claims = validationResult.claims;

        var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
    }
}


public class SiweAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "Siwe";
    public string TokenHeaderName { get; set; } = "Siwe";
    public string ApplicationName { get; set; }
    public string[] ValidIssuers { get; set; }
}