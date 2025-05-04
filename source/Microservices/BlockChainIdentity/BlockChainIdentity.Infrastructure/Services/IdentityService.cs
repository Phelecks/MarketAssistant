using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Nethereum.Siwe;
using Nethereum.Siwe.Core;
using Nethereum.Util;
using System.Security.Claims;
using BaseApplication.Helpers;
using BaseApplication.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace BlockChainIdentity.Infrastructure.Services;

public class IdentityService(SiweMessageService siweMessageService, IConfiguration configuration, ICypherService cypherService, IDateTimeService dateTimeService, IOptions<Domain.ConfigurationOptions> options) : Application.Interfaces.IIdentityService
{
    private readonly SiweMessageService _siweMessageService = siweMessageService;
    private readonly IConfiguration _configuration = configuration;
    private readonly ICypherService _cypherService = cypherService;
    private readonly IDateTimeService _dateTimeService = dateTimeService;
    private readonly string _issuer = options.Value.Issuer;


    private const string ClaimTypeAddress = "address";
    private const string ClaimTypeNonce = "nonce";
    private const string ClaimTypeSignature = "signature";
    private const string ClaimTypeSiweExpiry = "siweExpiry";
    private const string ClaimTypeSiweIssuedAt = "siweIssueAt";
    private const string ClaimTypeSiweNotBefore = "siweNotBefore";
    private const string ClaimTypeRoles = "role";
    private const string ClaimTypeAudience = "aud";
    private const string ClaimTypePolicies = "policy";
    private const string ClaimTypeUserId = "id";
    private const string ClaimTypeIssuer = "iss";
    private const string ClaimTypeUri = "uri";
    private const string ClaimTypeVersion = "version";
    private const string ClaimTypeChainId = "chainId";
    private const string ClaimTypeRequestId = "requestId";
    private const string ClaimTypeStatement = "statement";

    public async Task<(string token, SecurityTokenDescriptor tokenDescriptor)> GenerateTokenAsync(
        SiweMessage siweMessage, string signature, List<string> roles, List<string> resources, List<string> policies, 
        Uri clientUri, string version, int chainId, string requestId,
        string statement, CancellationToken cancellationToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = System.Text.Encoding.ASCII.GetBytes(GetSecret());
        var symetricSecureKey = new SymmetricSecurityKey(key);

        var claims = new HashSet<Claim>();
        foreach (var resource in resources)
            claims.Add(new Claim(ClaimTypeAudience, resource));
        foreach (var policy in policies)
            claims.Add(new Claim(ClaimTypePolicies, policy));
        foreach(var role in roles)
            claims.Add(new Claim(ClaimTypeRoles, role));
        claims.Add(new Claim(ClaimTypeAddress, siweMessage.Address));
        claims.Add(new Claim(ClaimTypeNonce, siweMessage.Nonce));
        claims.Add(new Claim(ClaimTypeSignature, signature));
        claims.Add(new Claim(ClaimTypeSiweExpiry, siweMessage.ExpirationTime));
        claims.Add(new Claim(ClaimTypeSiweIssuedAt, siweMessage.IssuedAt));
        claims.Add(new Claim(ClaimTypeSiweNotBefore, siweMessage.NotBefore));
        claims.Add(new Claim(ClaimTypeUserId, siweMessage.Address));
        claims.Add(new Claim("amr", "pwd"));
        claims.Add(new Claim("jti", siweMessage.Nonce));
        claims.Add(new Claim("idp", "local"));
        claims.Add(new Claim(ClaimTypeIssuer, _issuer));
        claims.Add(new Claim(ClaimTypeUri, clientUri.ToString()));
        claims.Add(new Claim(ClaimTypeVersion, version));
        claims.Add(new Claim(ClaimTypeChainId, chainId.ToString()));
        claims.Add(new Claim(ClaimTypeRequestId, requestId));
        claims.Add(new Claim(ClaimTypeStatement, statement));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            TokenType = "SIWE",
            Subject = new ClaimsIdentity(claims.ToArray()),
            SigningCredentials = new SigningCredentials(symetricSecureKey, SecurityAlgorithms.HmacSha256Signature)
        };
        if (!string.IsNullOrEmpty(siweMessage.ExpirationTime))
        {
            tokenDescriptor.Expires = GetIso8602AsDateTime(siweMessage.ExpirationTime);
        }
        if (!string.IsNullOrEmpty(siweMessage.IssuedAt))
        {
            tokenDescriptor.IssuedAt = GetIso8602AsDateTime(siweMessage.IssuedAt);
        }
        if (!string.IsNullOrEmpty(siweMessage.NotBefore))
        {
            tokenDescriptor.NotBefore = GetIso8602AsDateTime(siweMessage.NotBefore);
        }

        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        return await Task.FromResult((tokenHandler.WriteToken(token), tokenDescriptor));
    }

    public async Task<BaseResponseDto<SiweMessage>> ValidateTokenAsync(string token, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(token))
            return ResponseHelper.BadRequest<SiweMessage>("Token is null or empty.");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = System.Text.Encoding.ASCII.GetBytes(GetSecret());
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var address = jwtToken.Claims.First(x => x.Type == ClaimTypeAddress).Value;
            var nonce = jwtToken.Claims.First(x => x.Type == ClaimTypeNonce).Value;
            var issuedAt = jwtToken.Claims.First(x => x.Type == ClaimTypeSiweIssuedAt).Value;
            var expiry = jwtToken.Claims.First(x => x.Type == ClaimTypeSiweExpiry).Value;
            var notBefore = jwtToken.Claims.First(x => x.Type == ClaimTypeSiweNotBefore).Value;
            var roles = jwtToken.Claims.Where(x => x.Type == ClaimTypeRoles).ToList();
            var id = jwtToken.Claims.First(x => x.Type == ClaimTypeUserId).Value;
            var policies = jwtToken.Claims.Where(x => x.Type == ClaimTypePolicies).ToList();
            var issuer = jwtToken.Claims.First(x => x.Type == ClaimTypeIssuer).Value;
            var audiences = jwtToken.Claims.Where(x => x.Type == ClaimTypeAudience).ToList();
            var uri = new Uri(jwtToken.Claims.First(x => x.Type == ClaimTypeUri).Value);
            var version = jwtToken.Claims.First(x => x.Type == ClaimTypeVersion).Value;
            var chainId = jwtToken.Claims.First(x => x.Type == ClaimTypeChainId).Value;
            var requestId = jwtToken.Claims.First(x => x.Type == ClaimTypeRequestId).Value;
            var statement = jwtToken.Claims.First(x => x.Type == ClaimTypeStatement).Value;

            var signature = jwtToken.Claims.First(x => x.Type == ClaimTypeSignature).Value;


            var siweMessage = new SiweMessage
            {
                Address = address,
                Domain = uri.Authority,
                Statement = statement,
                Uri = uri.AbsoluteUri,
                Version = version,
                ChainId = chainId,
                RequestId = requestId,
                Nonce = nonce,
                ExpirationTime = expiry,
                NotBefore = notBefore,
                IssuedAt = issuedAt
            };
            //We could use the values stored in the jwt token but if NotBefore or Expiration are not set this will be defaulted
            //and we may not want to expire and renew it
            //also milliseconds are not set in the jwtToken so this causes a validation failure, for this to match milliseconds have to be zero

            if (await _siweMessageService.IsMessageSignatureValid(siweMessage, signature))
            {
                if (_siweMessageService.IsMessageTheSameAsSessionStored(siweMessage))
                {
                    if (_siweMessageService.HasMessageDateStartedAndNotExpired(siweMessage))
                    {
                        //Validate client resource
                        //Validate policies
                        //validate roles
                        return ResponseHelper.Success(siweMessage);
                    }
                    else
                        return ResponseHelper.BadRequest<SiweMessage>("Message date is not started or expired.");
                }
                else
                    return ResponseHelper.BadRequest<SiweMessage>("Message address and session address are not the same.");
            }
            else
                return ResponseHelper.BadRequest<SiweMessage>("Message signature is not valid.");
        }
        catch(Exception exception)
        {
            // return null if validation fails
            return ResponseHelper.Error<SiweMessage>(exception.Message);
        }
    }

    public string GenerateSiweMessage(string address, Uri uri, string statement, string version, int chainId, string requestId, DateTime expireDateTime)
    {
        var siweMessage = new SiweMessage
        {
            Address = address.ConvertToEthereumChecksumAddress(),
            Domain = uri.Authority,
            Statement = statement,
            Uri = uri.AbsoluteUri,
            Version = version,
            ChainId = chainId.ToString(),
            RequestId = requestId.ToString(),
        };
        siweMessage.SetExpirationTime(expireDateTime);
        siweMessage.SetNotBefore(_dateTimeService.UtcNow);
        var message = _siweMessageService.BuildMessageToSign(siweMessage);
        return message;
    }

    public BaseResponseDto<(string ClientId, string ClientSecret)> GetClient(string base64ClientKey)
    {
        var result = _cypherService.ConvertBase64ToString(base64ClientKey);
        var clientInfo = result.Split(':');
        if (clientInfo.Length != 2) return ResponseHelper.BadRequest<(string ClientId, string ClientSecret)>("Invalid cliendKey.");

        return ResponseHelper.Success<(string ClientId, string ClientSecret)>((clientInfo[0], clientInfo[1]));
    }




    protected static DateTime GetIso8602AsDateTime(string iso8601dateTime)
    {
        return DateTime.ParseExact(iso8601dateTime, "o",
            System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime();
    }
    string GetSecret()
    {
        return _configuration.GetValue("IDENTITY-SECRET", "DefaultSecretValue123")!;
    }
}