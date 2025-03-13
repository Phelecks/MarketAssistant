using BaseDomain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Informing.Domain.Entities;


public class ContactInformation : BaseEntity
{
    [ForeignKey("contact")]
    public long ContactId { get; set; }
    public virtual required Contact Contact { get; set; }

    [ForeignKey("Information")]
    public long InfromationId { get; set; }
    public virtual Information Information { get; set; }
}