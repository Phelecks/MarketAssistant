using BaseApplication.Mappings;
using BaseDomain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.BaseParameter.Queries.GetBaseParameter;

public class BaseParameterDto : IMapFrom<Domain.Entities.BaseParameter>
{
    public long id { get; set; }
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
