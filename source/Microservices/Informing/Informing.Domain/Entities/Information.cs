using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Informing.Domain.Entities;

public class Information : BaseAuditEntity
{
    /// <summary>
    /// Title
    /// </summary>
    [Required]
    public required string Title { get; set; }

    /// <summary>
    /// Content
    /// </summary>
    [Required]
    public required string Content { get; set; }

    /// <summary>
    /// Information type
    /// </summary>
    public InformationType Type { get; set; }

    /// <summary>
    /// Is enable
    /// </summary>
    public bool Enabled { get; set; } = true;

    public virtual ICollection<ContactInformation> ContactInformations { get; set; }
    public virtual ICollection<GroupInformation> GroupInformations { get; set; }
    public virtual ICollection<InformationLog> InformationLogs { get; set; }
}





/// <summary>
/// Information type
/// </summary>
public enum InformationType
{
    /// <summary>
    /// Sms, this will send with SMS to the user
    /// </summary>
    Sms,
    /// <summary>
    /// Email, this will send with email to the user
    /// </summary>
    Email,
    /// <summary>
    /// Notification, this will send as a notification to user applications
    /// </summary>
    Notification,
    /// <summary>
    /// News, this will store as a news in database and user can read it in news section of end user application
    /// </summary>
    News
}