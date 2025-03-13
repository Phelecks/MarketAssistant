using Microsoft.EntityFrameworkCore;

namespace Informing.Application.Interfaces;

public interface IApplicationDbContext : BaseApplication.Interfaces.IBaseApplicationDbContext
{
    DbSet<Domain.Entities.BaseParameter> BaseParameters { get; }
    DbSet<Domain.Entities.Contact> Contacts { get; }
    DbSet<Domain.Entities.Information> Information { get; }
    DbSet<Domain.Entities.InformationLog> InformationLogs { get; }
    DbSet<Domain.Entities.Template> Templates { get; }
    DbSet<Domain.Entities.Group> Groups { get; }
    DbSet<Domain.Entities.GroupContact> GroupContacts { get; }
    DbSet<Domain.Entities.Device> Devices { get; }
    DbSet<Domain.Entities.ContactInformation> ContactInformations { get; }
    DbSet<Domain.Entities.GroupInformation> GroupInformations { get; }
}
