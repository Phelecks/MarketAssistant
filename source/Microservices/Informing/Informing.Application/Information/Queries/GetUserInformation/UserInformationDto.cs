using BaseApplication.Mappings;
using Informing.Domain.Entities;

namespace Informing.Application.Information.Queries.GetUserInformation;

public class UserInformationDto : IMapFrom<Domain.Entities.Information>
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
    public InformationType type { get; set; }

    public DateTime created { get; set; }
}
