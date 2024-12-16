using BaseApplication.Helpers;
using Microsoft.IdentityModel.Tokens;
using Nethereum.Siwe.Core;

namespace BlockChainIdentity.Application.Interfaces;

public interface IIdentityService
{
    Task<(string token, SecurityTokenDescriptor tokenDescriptor)> GenerateTokenAsync(
        SiweMessage siweMessage, string signature, List<string> roles, List<string> resources, List<string> policies, 
        Uri clientUri, string version, int chainId, string requestId,
        string statement, CancellationToken cancellationToken);
    
    Task<BaseResponseDto<SiweMessage>> ValidateTokenAsync(string token, CancellationToken cancellationToken);
   
    string GenerateSiweMessage(string address, Uri uri, string statement, string version, int chainId, string requestId, DateTime expireDateTime);
    
    BaseResponseDto<(string ClientId, string ClientSecret)> GetClient(string base64ClientKey);
}