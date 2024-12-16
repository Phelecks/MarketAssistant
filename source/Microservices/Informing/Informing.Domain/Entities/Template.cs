using BaseDomain.Common;

namespace Informing.Domain.Entities;

public class Template : BaseEntity
{
    /// <summary>
    /// Title
    /// </summary>
    public string title { get; set; }

    /// <summary>
    /// Content
    /// </summary>
    public string content { get; set; }

    /// <summary>
    /// Information type
    /// </summary>
    public BaseDomain.Enums.InformingEnums.InformingType informingType { get; set; }

    /// <summary>
    /// Publication type
    /// </summary>
    public BaseDomain.Enums.InformingEnums.InformingSendType informingSendType { get; set; }
}