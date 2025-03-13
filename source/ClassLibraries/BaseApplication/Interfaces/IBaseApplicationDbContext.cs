namespace BaseApplication.Interfaces;

public interface IBaseApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
