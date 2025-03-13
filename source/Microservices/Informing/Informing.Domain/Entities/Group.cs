using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;

namespace Informing.Domain.Entities;


public class Group : BaseEntity
{
    [Required]
    public required string Title { get; set; }

    public string? Description { get; set; }






    public virtual ICollection<GroupContact> GroupContacts { get; set; }
}