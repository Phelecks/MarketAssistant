using BaseDomain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Informing.Domain.Entities;

public class Information : BaseAuditEntity
{
    /// <summary>
    /// Title
    /// </summary>
    public string title { get; set; }

    /// <summary>
    /// Content
    /// </summary>
    public string content { get; set; }

    ///// <summary>
    ///// Destination (Sms = phone number, Email = email address)
    ///// </summary>
    //public string? destination { get; set; }

    /// <summary>
    /// Information type
    /// </summary>
    public InformationType type { get; set; }

    /// <summary>
    /// Is enable
    /// </summary>
    public bool enabled { get; set; } = true;

    public virtual ICollection<ContactInformation> contactInformations { get; set; }
    public virtual ICollection<GroupInformation> groupInformations { get; set; }
    public virtual ICollection<InformationLog> informationLogs { get; set; }
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