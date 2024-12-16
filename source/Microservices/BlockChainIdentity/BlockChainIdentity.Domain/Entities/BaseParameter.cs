using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;
using BaseDomain.Enums;

namespace BlockChainIdentity.Domain.Entities;

/// <summary>
/// Base parameter
/// </summary>
public class BaseParameter : BaseAuditEntity
{
    /// <summary>
    /// Category
    /// </summary>
    [Required]
    public BaseParameterCategory category { get; set; }

    /// <summary>
    /// Field
    /// </summary>
    [Required]
    public BaseParameterField field { get; set; }

    /// <summary>
    /// Value
    /// </summary>
    [Required]
    public string value { get; set; }

    /// <summary>
    /// Kernel base parameter identifier
    /// </summary>
    [Required]
    public long kernelBaseParameterId { get; set; }
}