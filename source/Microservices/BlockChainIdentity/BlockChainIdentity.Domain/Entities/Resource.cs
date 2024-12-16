using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Domain.Entities;

public class Resource : BaseAuditEntity
{
    [Required]
    public string title { get; set; }

    public virtual ICollection<ClientResource> clientResources { get; set; }
}