using BaseApplication.Mappings;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.WalletRole.Queries.GetWalletRoles;

public class WalletRoleDto : IMapFrom<Domain.Entities.Role>
{
    public long id { get; set; }
    [Required]
    public string title { get; set; }
}
