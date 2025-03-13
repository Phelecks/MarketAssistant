using BaseDomain.Common;

namespace Informing.Domain.Entities;


public class GroupContact : BaseEntity
{
    public long ContactId { get; set; }
    public virtual Contact Contact { get; set; }



    public long GroupId { get; set; }
    public virtual Group Group { get; set; }
}