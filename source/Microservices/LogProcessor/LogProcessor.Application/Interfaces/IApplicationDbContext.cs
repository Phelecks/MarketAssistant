using Microsoft.EntityFrameworkCore;

namespace LogProcessor.Application.Interfaces;

public interface IApplicationDbContext : BaseApplication.Interfaces.IBaseApplicationDbContext
{
    DbSet<Domain.Entities.BlockProgress> BlockProgresses { get; }
    DbSet<Domain.Entities.Token> Tokens { get; }
    DbSet<Domain.Entities.RpcUrl> RpcUrls { get; }
    DbSet<Domain.Entities.Transfer> Transfers { get; }
    DbSet<Domain.Entities.Erc20Transfer> Erc20Transfers { get; }
    DbSet<Domain.Entities.Erc721Transfer> Erc721Transfers { get; }
}
