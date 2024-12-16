using BaseApplication.Mappings;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.Role.Queries.GetRoles;

public class RoleDto : IMapFrom<Domain.Entities.Role>
{
    public long id { get; set; }
    [Required]
    public string title { get; set; }
}
