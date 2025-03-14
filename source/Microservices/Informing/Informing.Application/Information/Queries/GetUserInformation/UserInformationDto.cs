using BaseApplication.Mappings;
using Informing.Domain.Entities;

namespace Informing.Application.Information.Queries.GetUserInformation;

public class UserInformationDto : IMapFrom<Domain.Entities.Information>
{
    /// <summary>
    /// Title
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Content
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    /// Information type
    /// </summary>
    public InformationType type { get; set; }

    public DateTime created { get; set; }
}
