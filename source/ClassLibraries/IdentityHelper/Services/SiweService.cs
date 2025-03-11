using Nethereum.Siwe;
using Nethereum.Siwe.Core;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using static IdentityHelper.Services.ISiweService;

namespace IdentityHelper.Services;

public class SiweService : ISiweService
{
    private readonly SiweMessageService _siweMessageService;

    private const string ClaimTypeAddress = "address";
    private const string ClaimTypeNonce = "nonce";
    private const string ClaimTypeSignature = "signature";
    private const string ClaimTypeSiweExpiry = "siweExpiry";
    private const string ClaimTypeSiweIssuedAt = "siweIssueAt";
    private const string ClaimTypeSiweNotBefore = "siweNotBefore";
    private const string ClaimTypeRoles = "role";
    private const string ClaimTypeAudience =  "aud";
    private const string ClaimTypePolicies = "policy";
    private const string ClaimTypeUserId = "id";
    private const string ClaimTypeIssuer = "iss";
    private const string ClaimTypeUri = "uri";
    private const string ClaimTypeVersion = "version";
    private const string ClaimTypeChainId = "chainId";
    private const string ClaimTypeRequestId = "requestId";
    private const string ClaimTypeStatement = "statement";

    public SiweService(SiweMessageService siweMessageService)
    {
        _siweMessageService = siweMessageService;
    }

    public async Task<ValidationResponse> ValidateSiweAsync(string token, string securityKey, string applicationName, string[] validIssuers)
    {
        if (string.IsNullOrEmpty(token))
            return new ValidationResponse(false, null, "Siwe is null or empty.");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(securityKey);
        try
        {
            tokenHandler.ValidateToken(token, new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuers = validIssuers,
                ValidateAudience = true,
                ValidAudience = applicationName,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out Microsoft.IdentityModel.Tokens.SecurityToken validatedToken);

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

            //if (jwtToken.IssuedAt != DateTime.MinValue)
            //{
            //    siweMessage.SetIssuedAt(jwtToken.IssuedAt);
            //}

            //if (jwtToken.ValidFrom != DateTime.MinValue)
            //{
            //    siweMessage.SetNotBefore(jwtToken.ValidFrom);
            //}

            //if (jwtToken.ValidTo != DateTime.MinValue)
            //{
            //    siweMessage.SetExpirationTime(jwtToken.ValidTo);
            //}

            //Debug.WriteLine(SiweMessageStringBuilder.BuildMessage(siweMessage));
            if (await _siweMessageService.IsMessageSignatureValid(siweMessage, signature))
            {
                if (_siweMessageService.IsMessageTheSameAsSessionStored(siweMessage))
                {
                    if (_siweMessageService.HasMessageDateStartedAndNotExpired(siweMessage))
                    {
                        ////Validate client resource
                        //if(!audiences.Any(x => x.Value.Equals(applicationName, StringComparison.InvariantCultureIgnoreCase)))
                        //    return new ValidationResponse(false, null, "User does not access to this resource.");

                        return new ValidationResponse(true, jwtToken.Claims, null);
                    }
                    else
                        return new ValidationResponse(false, null, "Message date is not started or expired.");
                }
                else
                    return new ValidationResponse(false, null, "Message address and session address are not the same.");
            }
            else
                return new ValidationResponse(false, null, "Message signature is not valid.");
        }
        catch (Exception exception)
        {
            // return null if validation fails
            return new ValidationResponse(false, null, exception.Message);
        }
    }
}