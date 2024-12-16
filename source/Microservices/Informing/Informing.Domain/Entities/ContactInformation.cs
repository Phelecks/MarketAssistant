using BaseDomain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Informing.Domain.Entities;


public class ContactInformation : BaseEntity
{
    [ForeignKey("contact")]
    public long contactId { get; set; }
    public virtual Contact contact { get; set; }

    [ForeignKey("information")]
    public long infromationId { get; set; }
    public virtual Information information { get; set; }
}