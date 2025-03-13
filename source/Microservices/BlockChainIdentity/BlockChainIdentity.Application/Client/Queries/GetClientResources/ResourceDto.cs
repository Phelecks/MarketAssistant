using BaseApplication.Mappings;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.Client.Queries.GetClientResources;

public class ResourceDto : IMapFrom<Domain.Entities.Resource>
{
    public long Id { get; set; }
    [Required]
    public required string Title { get; set; }
}
