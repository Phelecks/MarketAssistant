using BaseDomain.Common;

namespace Informing.Domain.Entities;


public class GroupContact : BaseEntity
{
    public long contactId { get; set; }
    public virtual Contact contact { get; set; }



    public long groupId { get; set; }
    public virtual Group group { get; set; }
}