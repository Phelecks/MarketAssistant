using BaseApplication.Mappings;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.Client.Queries.GetClient;

public class ClientDto :  IMapFrom<Domain.Entities.Client>
{
    public long id { get; set; }
    [Required]
    public string clientId { get; set; }

    [Required]
    public string clientSecret { get; set; }

    public Uri uri { get; set; }

    [Required]
    public int tokenLifeTimeInSeconds { get; set; }

    public bool enabled { get; set; }

    [Required]
    public string statement { get; set; }

    [Required]
    public string version { get; set; }
}
