using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlockChainIdentity.Grpc.Controllers.V1.Models;

/// <summary>
/// Get token data transfer object
/// </summary>
public class GetTokenRequest
{
    /// <summary>
    /// Siwe encoded message
    /// </summary>
    [Required]
    public required string SiweEncodedMessage { get; set; }

    /// <summary>
    /// Signature created by Signer
    /// </summary>
    [Required]
    public required string Signature { get; set; }

    /// <summary>
    /// Chain
    /// </summary>
    [Required]
    [JsonRequired]
    public int ChainId { get; set; }
}