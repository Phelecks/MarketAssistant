using System.ComponentModel.DataAnnotations;

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
    public string SiweEncodedMessage { get; set; }

    /// <summary>
    /// Signature created by Signer
    /// </summary>
    [Required]
    public string Signature { get; set; }

    /// <summary>
    /// Chain
    /// </summary>
    public int ChainId { get; set; }
}