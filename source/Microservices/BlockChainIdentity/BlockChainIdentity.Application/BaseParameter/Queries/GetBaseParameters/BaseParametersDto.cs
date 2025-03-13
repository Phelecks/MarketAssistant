using BaseApplication.Mappings;
using BaseDomain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.BaseParameter.Queries.GetBaseParameters;

public class BaseParametersDto : IMapFrom<Domain.Entities.BaseParameter>
{
    public long id { get; set; }
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
