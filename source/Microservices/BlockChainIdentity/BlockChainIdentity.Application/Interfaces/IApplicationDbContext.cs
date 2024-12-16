using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Interfaces;

public interface IApplicationDbContext : BaseApplication.Interfaces.IBaseApplicationDbContext
{
    DbSet<Domain.Entities.BaseParameter> baseParameters { get; }
    DbSet<Domain.Entities.Wallet> wallets { get; }
    DbSet<Domain.Entities.Token> tokens { get; }
    DbSet<Domain.Entities.Client> clients { get; }
    DbSet<Domain.Entities.Resource> resources { get; }
    DbSet<Domain.Entities.ClientResource> clientResources { get; }
    DbSet<Domain.Entities.Role> roles { get; }
    DbSet<Domain.Entities.WalletRole> walletRoles { get; }
}
