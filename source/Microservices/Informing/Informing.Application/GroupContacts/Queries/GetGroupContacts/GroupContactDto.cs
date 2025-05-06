using BaseApplication.Mappings;

namespace Informing.Application.GroupContacts.Queries.GetGroupContacts;

public class GroupContactDto : Domain.Entities.GroupContact, IMapFrom<Domain.Entities.GroupContact>
{
}
