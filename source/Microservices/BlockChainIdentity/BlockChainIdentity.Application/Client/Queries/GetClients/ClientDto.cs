using BaseApplication.Mappings;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.Client.Queries.GetClients;

public class ClientDto : IMapFrom<Domain.Entities.Client>
{
    public long Id { get; set; }
    
    [Required]
    public required string ClientId { get; set; }

    [Required]
    public required string ClientSecret { get; set; }

    public required Uri Uri { get; set; }

    [Required]
    public int TokenLifeTimeInSeconds { get; set; }

    public bool Enabled { get; set; }

    [Required]
    public required string Statement { get; set; }

    [Required]
    public required string Version { get; set; }
}
