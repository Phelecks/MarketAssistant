using Microsoft.EntityFrameworkCore;

namespace BlockProcessor.Application.Interfaces;

public interface IApplicationDbContext : BaseApplication.Interfaces.IBaseApplicationDbContext
{
    DbSet<Domain.Entities.BlockProgress> BlockProgresses { get; }
    DbSet<Domain.Entities.WalletAddress> WalletAddresses { get; }
    DbSet<Domain.Entities.RpcUrl> RpcUrls { get; }
    DbSet<Domain.Entities.Transfer> Transfers { get; }
    DbSet<Domain.Entities.Erc20Transfer> Erc20Transfers { get; }
    DbSet<Domain.Entities.Erc721Transfer> Erc721Transfers { get; }
}
