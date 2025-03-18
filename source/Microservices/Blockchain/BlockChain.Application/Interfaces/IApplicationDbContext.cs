using Microsoft.EntityFrameworkCore;

namespace BlockChain.Application.Interfaces;

public interface IApplicationDbContext : BaseApplication.Interfaces.IBaseApplicationDbContext
{
    DbSet<Domain.Entities.Customer> Customers { get; }
    DbSet<Domain.Entities.Track> Tracks { get; }
    DbSet<Domain.Entities.Notification> Notifications { get; }
}
