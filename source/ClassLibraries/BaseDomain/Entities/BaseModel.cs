using System.ComponentModel.DataAnnotations;

namespace BaseDomain.Entities;

public class BaseModel
{
    /// <summary>
    /// Title
    /// </summary>
    [MaxLength(128, ErrorMessage = "Title must be less than 128 characters")]
    public string? title { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    [DataType(DataType.MultilineText)]
    public string? description { get; set; }
}