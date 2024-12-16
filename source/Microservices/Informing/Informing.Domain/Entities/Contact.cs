using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;

namespace Informing.Domain.Entities;

/// <summary>
/// Contact
/// </summary>
public class Contact : BaseAuditEntity
{
    /// <summary>
    /// User identifier
    /// </summary>
    [Required]
    public string userId { get; set; }

    /// <summary>
    /// Phone Number
    /// </summary>
    public string? phoneNumber { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string? emailAddress { get; set; }

    /// <summary>
    /// Username, it comes from identity microservice
    /// </summary>
    [Required]
    public string username { get; set; }

    /// <summary>
    /// Full Name, it come from customer microservice
    /// </summary>
    public string? fullname { get; set; } = null;




    public virtual ICollection<InformationLog> informationLogs { get; set; }
    public virtual ICollection<GroupContact> groupContacts { get; set; }
    public virtual ICollection<Device> devices { get; set; }
}