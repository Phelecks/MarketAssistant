using BaseApplication.Mappings;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.Resource.Queries.GetResource;

public class ResourceDto : IMapFrom<Domain.Entities.Resource>
{
    public long Id { get; set; }

    [Required]
    public required string Title { get; set; }
}
