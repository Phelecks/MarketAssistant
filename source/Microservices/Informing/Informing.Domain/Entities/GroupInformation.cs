using BaseDomain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Informing.Domain.Entities;


public class GroupInformation : BaseEntity
{
    [ForeignKey("group")]
    public long groupId { get; set; }
    public virtual Group group { get; set; }

    [ForeignKey("information")]
    public long infromationId { get; set; }
    public virtual Information information { get; set; }
}