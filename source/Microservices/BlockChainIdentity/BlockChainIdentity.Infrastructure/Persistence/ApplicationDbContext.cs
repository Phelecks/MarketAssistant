using System.Reflection;
using BaseDomain.Helpers;
using BlockChainIdentity.Application.Interfaces;
using BlockChainIdentity.Domain.Entities;
using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BlockChainIdentity.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IEncryptionProvider _encryptionProvider;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
    {
        var encryptionKey = configuration.GetValue<string>("DATABASE-ENCRYPTION-KEY");
        _encryptionProvider = new GenerateEncryptionProvider(encryptionKey);
    }

    public DbSet<BaseParameter> baseParameters => Set<BaseParameter>();
    public DbSet<Client> clients => Set<Client>();
    public DbSet<ClientResource> clientResources => Set<ClientResource>();
    public DbSet<Resource> resources => Set<Resource>();
    public DbSet<Domain.Entities.Role> roles => Set<Domain.Entities.Role>();
    public DbSet<Token> tokens => Set<Token>();
    public DbSet<Domain.Entities.Wallet> wallets => Set<Domain.Entities.Wallet>();
    public DbSet<WalletRole> walletRoles => Set<WalletRole>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.UseEncryption(_encryptionProvider);
        base.OnModelCreating(modelBuilder);
    }
}
