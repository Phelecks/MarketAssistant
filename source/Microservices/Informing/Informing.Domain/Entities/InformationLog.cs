using BaseDomain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Informing.Domain.Entities;

public class InformationLog : BaseAuditEntity
{
    public InformationLogType type { get; set; }

    [ForeignKey("contact")]
    public long contactId { get; set; }
    public virtual Contact contact { get; set; }

    [ForeignKey("information")]
    public long informationId { get; set; }
    public virtual Information information { get; set; }
}


public enum InformationLogType
{
    Read
}