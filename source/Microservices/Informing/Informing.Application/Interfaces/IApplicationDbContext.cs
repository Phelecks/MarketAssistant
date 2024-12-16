using Microsoft.EntityFrameworkCore;

namespace Informing.Application.Interfaces;

public interface IApplicationDbContext : BaseApplication.Interfaces.IBaseApplicationDbContext
{
    DbSet<Domain.Entities.BaseParameter> baseParameters { get; }
    DbSet<Domain.Entities.Contact> contacts { get; }
    DbSet<Domain.Entities.Information> information { get; }
    DbSet<Domain.Entities.InformationLog> informationLogs { get; }
    DbSet<Domain.Entities.Template> templates { get; }
    DbSet<Domain.Entities.Group> groups { get; }
    DbSet<Domain.Entities.GroupContact> groupContacts { get; }
    DbSet<Domain.Entities.Device> devices { get; }
    DbSet<Domain.Entities.ContactInformation> contactInformations { get; }
    DbSet<Domain.Entities.GroupInformation> groupInformations { get; }
}
