using System.Reflection;
using BlockProcessor.Application.Interfaces;
using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BlockProcessor.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IEncryptionProvider _encryptionProvider;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
    {
        var encryptionKey = configuration.GetValue<string>("DATABASE-ENCRYPTION-KEY");
        _encryptionProvider = new GenerateEncryptionProvider(encryptionKey);
    }

    public DbSet<Domain.Entities.BlockProgress> BlockProgresses => Set<Domain.Entities.BlockProgress>();
    public DbSet<Domain.Entities.WalletAddress> WalletAddresses => Set<Domain.Entities.WalletAddress>();
    public DbSet<Domain.Entities.RpcUrl> RpcUrls => Set<Domain.Entities.RpcUrl>();
    public DbSet<Domain.Entities.Transfer> Transfers => Set<Domain.Entities.Transfer>();
    public DbSet<Domain.Entities.Erc20Transfer> Erc20Transfers => Set<Domain.Entities.Erc20Transfer>();
    public DbSet<Domain.Entities.Erc721Transfer> Erc721Transfers => Set<Domain.Entities.Erc721Transfer>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.UseEncryption(_encryptionProvider);
        base.OnModelCreating(modelBuilder);
    }
}
