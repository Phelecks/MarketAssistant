using BaseDomain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Informing.Domain.Entities;


public class GroupInformation : BaseEntity
{
    [ForeignKey("group")]
    public long GroupId { get; set; }
    public virtual Group Group { get; set; }

    [ForeignKey("Information")]
    public long InfromationId { get; set; }
    public virtual Information Information { get; set; }
}