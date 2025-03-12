using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
    public required string Address { get; set; }

    /// <summary>
    /// Chain
    /// </summary>
    [Required]
    [JsonRequired]
    public int ChainId { get; set; }
}