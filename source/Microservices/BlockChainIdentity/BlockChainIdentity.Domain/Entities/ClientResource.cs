using BaseDomain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlockChainIdentity.Domain.Entities;

public class ClientResource : BaseEntity
{
    [ForeignKey("client")]
    public long clientId { get; set; }
    public virtual Client client { get; set; }

    [ForeignKey("resource")]
    public long resourceId { get; set; }
    public virtual Resource resource { get; set; }
}