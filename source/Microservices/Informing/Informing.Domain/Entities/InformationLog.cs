using BaseDomain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Informing.Domain.Entities;

public class InformationLog : BaseAuditEntity
{
    public InformationLogType Type { get; set; }

    [ForeignKey("Contact")]
    public long ContactId { get; set; }
    public virtual Contact Contact { get; set; }

    [ForeignKey("Information")]
    public long InformationId { get; set; }
    public virtual Information Information { get; set; }
}


public enum InformationLogType
{
    Read
}