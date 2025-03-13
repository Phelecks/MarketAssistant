using System.Reflection;
using Informing.Application.Interfaces;
using Informing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Informing.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<BaseParameter> BaseParameters => Set<BaseParameter>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Information> Information => Set<Information>();
    public DbSet<InformationLog> InformationLogs => Set<InformationLog>();
    public DbSet<Template> Templates => Set<Template>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<GroupContact> GroupContacts => Set<GroupContact>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<ContactInformation> ContactInformations => Set<ContactInformation>(); 
    public DbSet<GroupInformation> GroupInformations => Set<GroupInformation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
