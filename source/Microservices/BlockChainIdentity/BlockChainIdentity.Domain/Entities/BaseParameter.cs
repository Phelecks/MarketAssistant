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
    public BaseParameterCategory Category { get; set; }

    /// <summary>
    /// Field
    /// </summary>
    [Required]
    public BaseParameterField Field { get; set; }

    /// <summary>
    /// Value
    /// </summary>
    [Required]
    public required string Value { get; set; }

    /// <summary>
    /// Kernel base parameter identifier
    /// </summary>
    [Required]
    public long KernelBaseParameterId { get; set; }
}