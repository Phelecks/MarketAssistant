using System.Reflection;
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

    public DbSet<BaseParameter> BaseParameters => Set<BaseParameter>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<ClientResource> ClientResources => Set<ClientResource>();
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<Domain.Entities.Role> Roles => Set<Domain.Entities.Role>();
    public DbSet<Token> Tokens => Set<Token>();
    public DbSet<Wallet> Wallets => Set<Wallet>();
    public DbSet<WalletRole> WalletRoles => Set<WalletRole>();
    public DbSet<RpcUrl> RpcUrls => Set<RpcUrl>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.UseEncryption(_encryptionProvider);
        base.OnModelCreating(modelBuilder);
    }
}
