using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;

namespace Informing.Domain.Entities;

public class Device : BaseAuditEntity
{
    /// <summary>
    /// Operating system type
    /// </summary>
    public BaseDomain.Enums.InformingEnums.PlatformType PlatformType { get; set; }

    /// <summary>
    /// Operation system version
    /// </summary>
    public required string Version { get; set; } = "Unknown";

    /// <summary>
    /// Push notification identifier
    /// </summary>
    [Required]
    public required string DeviceToken { get; set; }

    /// <summary>
    /// Enabled
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Submit date time
    /// </summary>
    public DateTime SubmitDateTime { get; }
}