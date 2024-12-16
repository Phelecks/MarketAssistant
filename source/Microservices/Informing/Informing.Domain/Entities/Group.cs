using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;

namespace Informing.Domain.Entities;


public class Group : BaseEntity
{
    [Required]
    public string title { get; set; }

    public string? description { get; set; }






    public virtual ICollection<GroupContact> groupContacts { get; set; }
}