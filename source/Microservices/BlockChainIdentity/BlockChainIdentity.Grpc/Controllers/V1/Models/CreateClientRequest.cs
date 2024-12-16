using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BlockChainIdentity.Grpc.Controllers.V1.Models;

public class CreateClientRequest
{
    [Required]
    public string ClientId { get; set; }

    [Required]
    public string ClientSecret { get; set; }

    [Required]
    public Uri Uri { get; set; }

    public bool Enabled { get; set; }

    [Description("Sentence to aknowledge user about to connect an application.")]
    public string Statement { get; set; }

    [Required]
    public string Version { get; set; }

    public List<long> ResourceIds { get; set; }

    public int? TokenLifeTimeInSeconds { get; set; }
}