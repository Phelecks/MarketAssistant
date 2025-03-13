using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Domain.Entities;

public class Resource : BaseAuditEntity
{
    [Required]
    public required string Title { get; set; }

    public virtual ICollection<ClientResource> ClientResources { get; set; }
}