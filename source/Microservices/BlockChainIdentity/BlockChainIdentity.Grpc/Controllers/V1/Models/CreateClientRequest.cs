using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BlockChainIdentity.Grpc.Controllers.V1.Models;

public class CreateClientRequest
{
    [Required]
    public required string ClientId { get; set; }

    [Required]
    public required string ClientSecret { get; set; }

    [Required]
    public required Uri Uri { get; set; }

    [JsonRequired]
    public bool Enabled { get; set; }

    [Description("Sentence to aknowledge user about to connect an application.")]
    public required string Statement { get; set; }

    [Required]
    public required string Version { get; set; }

    public required List<long> ResourceIds { get; set; }

    public int? TokenLifeTimeInSeconds { get; set; }
}