using BaseDomain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlockChainIdentity.Domain.Entities;

public class ClientResource : BaseEntity
{
    [ForeignKey("Client")]
    public long ClientId { get; set; }
    public virtual Client Client { get; set; }

    [ForeignKey("Resource")]
    public long ResourceId { get; set; }
    public virtual Resource Resource { get; set; }
}