using BaseApplication.Mappings;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.Resource.Queries.GetResources;

public class ResourceDto : IMapFrom<Domain.Entities.Resource>
{
    public long id { get; set; }
    [Required]
    public string title { get; set; }
}
