using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Interfaces;

public interface IApplicationDbContext : BaseApplication.Interfaces.IBaseApplicationDbContext
{
    DbSet<Domain.Entities.Wallet> Wallets { get; }
    DbSet<Domain.Entities.Token> Tokens { get; }
    DbSet<Domain.Entities.Client> Clients { get; }
    DbSet<Domain.Entities.Resource> Resources { get; }
    DbSet<Domain.Entities.ClientResource> ClientResources { get; }
    DbSet<Domain.Entities.Role> Roles { get; }
    DbSet<Domain.Entities.WalletRole> WalletRoles { get; }
    DbSet<Domain.Entities.RpcUrl> RpcUrls { get; }
}
