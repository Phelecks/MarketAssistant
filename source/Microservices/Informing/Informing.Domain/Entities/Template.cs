using System.ComponentModel.DataAnnotations;
using BaseDomain.Common;

namespace Informing.Domain.Entities;

public class Template : BaseEntity
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
    public BaseDomain.Enums.InformingEnums.InformingType InformingType { get; set; }

    /// <summary>
    /// Publication type
    /// </summary>
    public BaseDomain.Enums.InformingEnums.InformingSendType InformingSendType { get; set; }
}