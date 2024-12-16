using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Grpc.Controllers.V1.Models;

/// <summary>
/// Get Siwe data transfer object
/// </summary>
public class GetSiweRequest
{
    /// <summary>
    /// Wallet Address
    /// </summary>
    [Required]
    public string Address { get; set; }

    /// <summary>
    /// Chain
    /// </summary>
    [Required]
    public int ChainId { get; set; }
}