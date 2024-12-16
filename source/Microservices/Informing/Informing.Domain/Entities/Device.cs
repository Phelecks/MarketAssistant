using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;

namespace Informing.Domain.Entities;

public class Device : BaseAuditEntity
{
    /// <summary>
    /// Operating system type
    /// </summary>
    public BaseDomain.Enums.InformingEnums.PlatformType platformType { get; set; }

    /// <summary>
    /// Operation system version
    /// </summary>
    public string version { get; set; }

    /// <summary>
    /// Push notification identifier
    /// </summary>
    [Required]
    public string deviceToken { get; set; }

    /// <summary>
    /// Enabled
    /// </summary>
    public bool enabled { get; set; } = true;

    /// <summary>
    /// Submit date time
    /// </summary>
    public DateTime submitDateTime { get; }
}