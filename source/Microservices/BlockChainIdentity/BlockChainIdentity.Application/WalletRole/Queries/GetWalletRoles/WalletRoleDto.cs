using BaseApplication.Mappings;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.WalletRole.Queries.GetWalletRoles;

public class WalletRoleDto : IMapFrom<Domain.Entities.Role>
{
    public long Id { get; set; }

    [Required]
    public required string Title { get; set; }
}
