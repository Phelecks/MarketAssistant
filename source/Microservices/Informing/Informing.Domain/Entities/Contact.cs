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
    public required string UserId { get; set; }

    /// <summary>
    /// Phone Number
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string? EmailAddress { get; set; }

    /// <summary>
    /// Username, it comes from identity microservice
    /// </summary>
    [Required]
    public required string Username { get; set; }

    /// <summary>
    /// Full Name, it come from customer microservice
    /// </summary>
    public string? Fullname { get; set; } = null;




    public virtual ICollection<InformationLog> InformationLogs { get; set; }
    public virtual ICollection<GroupContact> GroupContacts { get; set; }
    public virtual ICollection<Device> Devices { get; set; }
}