using System.Reflection;
using BlockChain.Application.Interfaces;
using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BlockChain.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IEncryptionProvider _encryptionProvider;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
    {
        var encryptionKey = configuration.GetValue<string>("DATABASE-ENCRYPTION-KEY");
        _encryptionProvider = new GenerateEncryptionProvider(encryptionKey);
    }

    public DbSet<Domain.Entities.Customer> Customers => Set<Domain.Entities.Customer>();
    public DbSet<Domain.Entities.Track> Tracks => Set<Domain.Entities.Track>();
    public DbSet<Domain.Entities.Notification> Notifications => Set<Domain.Entities.Notification>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.UseEncryption(_encryptionProvider);
        base.OnModelCreating(modelBuilder);
    }
}
