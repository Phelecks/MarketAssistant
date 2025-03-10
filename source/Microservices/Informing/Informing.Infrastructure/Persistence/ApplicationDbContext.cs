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

    public DbSet<BaseParameter> baseParameters => Set<BaseParameter>();
    public DbSet<Contact> contacts => Set<Contact>();
    public DbSet<Information> information => Set<Information>();
    public DbSet<InformationLog> informationLogs => Set<InformationLog>();
    public DbSet<Template> templates => Set<Template>();
    public DbSet<Group> groups => Set<Group>();
    public DbSet<GroupContact> groupContacts => Set<GroupContact>();
    public DbSet<Device> devices => Set<Device>();
    public DbSet<ContactInformation> contactInformations => Set<ContactInformation>(); 
    public DbSet<GroupInformation> groupInformations => Set<GroupInformation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
